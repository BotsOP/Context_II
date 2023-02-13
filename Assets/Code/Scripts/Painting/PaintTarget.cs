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
    [SerializeField] private int textureSize = 1024;
    [SerializeField] private Material DisplayMat;

    public GameObject particles;
    public VisualEffect vfx;
    
    private Renderer rend;
    private Material SetPaint;
    private ComputeShader CopyPaint;
    private RenderTexture rendTex;
    private RenderTexture rendTex2;
    private CommandBuffer commandBuffer;
    private int kernelID;
    private Vector2 threadGroupSize;

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
        
        SetPaint.SetTexture("_MainTex", rendTex);
        DisplayMat.SetTexture("_MainTex", rendTex2);
        DisplayMat.SetFloat("_Taintedness", taintedness);
        
        threadGroupSize.x = Mathf.CeilToInt((float)rendTex.width / threadGroupSizeX);
        threadGroupSize.y = Mathf.CeilToInt((float)rendTex.height / threadGroupSizeY);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            rendTex2.Release();
            DisplayMat.SetTexture("_MainTex", rendTex2);
            
            taintedness -= taintDepletionRate;
            taintedness = Mathf.Clamp01(taintedness);
            DisplayMat.SetFloat("_Taintedness", taintedness);
            
            vfx.SetInt("SpawnRate", (int)Mathf.Lerp(0, 500, taintedness));
            
            particles.SetActive(true);
        }
        else
        {
            particles.SetActive(false);
        }
    }

    public void Paint(Vector3 position, Color color, float hardness = 1, float strength = 1, float radius = 1)
    {
        kernelID = 0;
        SetPaint.SetVector("_PaintPos", position);
        SetPaint.SetFloat("_Hardness", hardness);
        SetPaint.SetFloat("_Strength", strength);
        SetPaint.SetFloat("_Radius", radius);
        SetPaint.SetColor("_Color", color);
        CopyPaint.SetTexture(kernelID, "newPaintTex", rendTex);
        CopyPaint.SetTexture(kernelID, "mainPaintTex", rendTex2);
        
        commandBuffer.SetRenderTarget(rendTex);
        commandBuffer.DrawRenderer(rend, SetPaint, 0);
        
        commandBuffer.DispatchCompute(CopyPaint, kernelID, (int)threadGroupSize.x, (int)threadGroupSize.y, 1);

        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();

        DisplayMat.SetTexture("_MainTex", rendTex2);
        taintedness += taintGainRate;
        taintedness = Mathf.Clamp01(taintedness);
        DisplayMat.SetFloat("_Taintedness", taintedness);
    }
}
