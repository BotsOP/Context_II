using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class PaintTarget : MonoBehaviour, IPaintable
{
    [SerializeField] private float taintedness;
    [SerializeField] private float taintDepletionRate = 0.1f;
    [SerializeField] private float taintGainRate = 0.1f;
    [SerializeField] private int amountOilBlobs = 500;
    [SerializeField] private float deactivationDelay = 4f;
    [SerializeField] private int textureSize = 1024;
    [SerializeField] private Material displayMatProperties;
    [SerializeField] private GameObject particles;
    
    private VisualEffect vfx;
    private Renderer rend;
    private Material SetPaint;
    private ComputeShader CopyPaint;
    private RenderTexture newPaintTex;
    private RenderTexture allPaintTex;
    private CommandBuffer commandBuffer;
    private int kernelID;
    private Vector2 threadGroupSize;
    private VFXPropertyBinder vfxBinder;
    private VFXTransformBinder2 suckerTransformBinding;
    private float timeSinceLastActivation;
    private bool isBeingSucked;
    private Shader paintableShader;
    private Material displayMat;

    #region shader properties
    private readonly static int MaskTexture = Shader.PropertyToID("_MaskTexture");
    private readonly static int Taintedness = Shader.PropertyToID("_Taintedness");
    private readonly static int Tex = Shader.PropertyToID("_MainTex");
    private readonly static int PaintPos = Shader.PropertyToID("_PaintPos");
    private readonly static int Hardness = Shader.PropertyToID("_Hardness");
    private readonly static int Strength = Shader.PropertyToID("_Strength");
    private readonly static int Radius = Shader.PropertyToID("_Radius");
    private readonly static int Color1 = Shader.PropertyToID("_Color");
    #endregion
    

    private void Awake()
    {
        vfx = particles.GetComponent<VisualEffect>();
        vfxBinder = particles.GetComponent<VFXPropertyBinder>();
        rend = GetComponent<MeshRenderer>();
        commandBuffer = new CommandBuffer();
        CopyPaint = Resources.Load<ComputeShader>("CopyPaint");
        SetPaint = new Material(Resources.Load<Shader>("SetPaint"));
        
        displayMat = new Material(displayMatProperties.shader);
        displayMatProperties.CopyPropertiesFromMaterial(displayMat);
        rend.material = displayMat;
        
        kernelID = 0;
        CopyPaint.GetKernelThreadGroupSizes(kernelID, out uint threadGroupSizeX, out uint threadGroupSizeY, out _);
        
        newPaintTex = new CustomRenderTexture(textureSize, textureSize, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        newPaintTex.enableRandomWrite = true;
        
        allPaintTex = new CustomRenderTexture(textureSize, textureSize, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        allPaintTex.enableRandomWrite = true;
        
        SetPaint.SetTexture(Tex, newPaintTex);
        displayMat.SetTexture(MaskTexture, allPaintTex);
        displayMat.SetFloat(Taintedness, taintedness);
        
        threadGroupSize.x = Mathf.CeilToInt((float)newPaintTex.width / threadGroupSizeX);
        threadGroupSize.y = Mathf.CeilToInt((float)newPaintTex.height / threadGroupSizeY);

        suckerTransformBinding = particles.AddComponent<VFXTransformBinder2>();
        suckerTransformBinding.Property = "SuckerTransform";
        vfxBinder.m_Bindings.Add(suckerTransformBinding);
    }

    private void Update()
    {
        CheckToDeactivateParticles();

        if (!isBeingSucked)
        {
            vfx.SetInt("SpawnRate", 0);
        }
    }

    private void CheckToDeactivateParticles()
    {
        if (Time.time > timeSinceLastActivation + deactivationDelay)
        {
            particles.SetActive(false);
            return;
        }
        particles.SetActive(true);
    }

    public void DecayPaint(float suckingForce = 1f)
    {
        kernelID = 1;
        CopyPaint.SetFloat("decayRate", taintDepletionRate * suckingForce);
        CopyPaint.SetTexture(kernelID, "mainPaintTex", allPaintTex);
        CopyPaint.Dispatch(kernelID, (int)threadGroupSize.x, (int)threadGroupSize.y, 1);
        
        //displayMat.SetTexture(MaskTexture, allPaintTex);
        
        taintedness -= taintDepletionRate * suckingForce;
        taintedness = Mathf.Clamp01(taintedness);
        displayMat.SetFloat(Taintedness, taintedness);
        int amountParticlesToSpawn = (int)Mathf.Lerp(-amountOilBlobs / 3.0f, amountOilBlobs, taintedness);
        //Debug.Log(amountParticlesToSpawn);
            
        vfx.SetInt("SpawnRate", amountParticlesToSpawn);
        vfx.SetTexture("MaskTexture", allPaintTex);
    }

    public float GetTaintedness()
    {
        return taintedness;
    }
    public void Paint(Vector3 position, Color color, float hardness = 1, float strength = 1, float radius = 1)
    {
        kernelID = 0;
        SetPaint.SetVector(PaintPos, position);
        SetPaint.SetFloat(Hardness, hardness);
        SetPaint.SetFloat(Strength, strength);
        SetPaint.SetFloat(Radius, radius);
        SetPaint.SetColor(Color1, color);
        CopyPaint.SetTexture(kernelID, "newPaintTex", newPaintTex);
        CopyPaint.SetTexture(kernelID, "mainPaintTex", allPaintTex);
        
        commandBuffer.SetRenderTarget(newPaintTex);
        commandBuffer.DrawRenderer(rend, SetPaint, 0);
        
        commandBuffer.DispatchCompute(CopyPaint, kernelID, (int)threadGroupSize.x, (int)threadGroupSize.y, 1);

        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();

        displayMat.SetTexture(MaskTexture, allPaintTex);
        taintedness += taintGainRate;
        taintedness = Mathf.Clamp01(taintedness);
        displayMat.SetFloat(Taintedness, taintedness);
    }

    public void SuckTarget(Transform suckTransform, float suckingForce = 1f)
    {
        isBeingSucked = true;
        timeSinceLastActivation = Time.timeSinceLevelLoad;
        suckerTransformBinding.Target = suckTransform;
        DecayPaint(suckingForce);
    }
    public void StoppedSucking()
    {
        isBeingSucked = false;
    }

    private Vector3 MultiplyVector3(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}
