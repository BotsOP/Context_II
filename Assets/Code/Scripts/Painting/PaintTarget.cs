using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class PaintTarget : MonoBehaviour, IPaintable
{
    [SerializeField] private float taintedness;
    [SerializeField] private float taintDepletionRate = 0.1f;
    [SerializeField] private float taintGainRate = 0.1f;
    [SerializeField] private int amountOilBlobs = 500;
    [SerializeField] private int textureSize = 1024;
    [SerializeField] private Material DisplayMat;

    public GameObject particles;
    public Transform sucker;
    public VisualEffect vfx;
    
    private Renderer rend;
    private Material SetPaint;
    private ComputeShader CopyPaint;
    private RenderTexture rendTex;
    private RenderTexture rendTex2;
    private CommandBuffer commandBuffer;
    private int kernelID;
    private Vector2 threadGroupSize;

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
        rend = GetComponent<MeshRenderer>();
        commandBuffer = new CommandBuffer();
        CopyPaint = Resources.Load<ComputeShader>("CopyPaint");
        SetPaint = new Material(Resources.Load<Shader>("SetPaint"));
        
        kernelID = 0;
        CopyPaint.GetKernelThreadGroupSizes(kernelID, out uint threadGroupSizeX, out uint threadGroupSizeY, out _);
        
        rendTex = new CustomRenderTexture(textureSize, textureSize, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        rendTex.enableRandomWrite = true;
        
        rendTex2 = new CustomRenderTexture(textureSize, textureSize, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        rendTex2.enableRandomWrite = true;
        
        SetPaint.SetTexture(Tex, rendTex);
        DisplayMat.SetTexture(MaskTexture, rendTex2);
        DisplayMat.SetFloat(Taintedness, taintedness);
        
        threadGroupSize.x = Mathf.CeilToInt((float)rendTex.width / threadGroupSizeX);
        threadGroupSize.y = Mathf.CeilToInt((float)rendTex.height / threadGroupSizeY);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            //rendTex2.Release();
            DecayPaint();
            DisplayMat.SetTexture(MaskTexture, rendTex2);
            
            taintedness -= taintDepletionRate;
            taintedness = Mathf.Clamp01(taintedness);
            DisplayMat.SetFloat(Taintedness, taintedness);
            uint amountParticlesToSpawn = (uint)Mathf.Lerp(-amountOilBlobs / 3.0f, amountOilBlobs, taintedness);
            Debug.Log((int)amountParticlesToSpawn);
            
            vfx.SetInt("SpawnRate", (int)amountParticlesToSpawn);
            vfx.SetTexture("MaskTexture", rendTex2);
            
            particles.SetActive(true);
        }
        else
        {
            particles.SetActive(false);
        }
    }

    public void DecayPaint()
    {
        kernelID = 1;
        CopyPaint.SetFloat("decayRate", taintDepletionRate);
        CopyPaint.SetTexture(kernelID, "mainPaintTex", rendTex2);
        CopyPaint.Dispatch(kernelID, (int)threadGroupSize.x, (int)threadGroupSize.y, 1);
    }

    public void Paint(Vector3 position, Color color, float hardness = 1, float strength = 1, float radius = 1)
    {
        kernelID = 0;
        SetPaint.SetVector(PaintPos, position);
        SetPaint.SetFloat(Hardness, hardness);
        SetPaint.SetFloat(Strength, strength);
        SetPaint.SetFloat(Radius, radius);
        SetPaint.SetColor(Color1, color);
        CopyPaint.SetTexture(kernelID, "newPaintTex", rendTex);
        CopyPaint.SetTexture(kernelID, "mainPaintTex", rendTex2);
        
        commandBuffer.SetRenderTarget(rendTex);
        commandBuffer.DrawRenderer(rend, SetPaint, 0);
        
        commandBuffer.DispatchCompute(CopyPaint, kernelID, (int)threadGroupSize.x, (int)threadGroupSize.y, 1);

        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();

        DisplayMat.SetTexture(MaskTexture, rendTex2);
        taintedness += taintGainRate;
        taintedness = Mathf.Clamp01(taintedness);
        DisplayMat.SetFloat(Taintedness, taintedness);
    }

    private Vector3 MultiplyVector3(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}
