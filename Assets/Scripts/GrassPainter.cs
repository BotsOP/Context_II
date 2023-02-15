using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GrassPainter : MonoBehaviour
{
    [SerializeField] private ComputeShader shader;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private float grassRadius;
    public bool shouldCheck;
    private Mesh mesh;
    private GraphicsBuffer gpuVertices;
    private GraphicsBuffer gpuUV;
    private GraphicsBuffer gpuIndices;
    private int kernelID;
    private int threadGroupSize;
    private int cachedPlanetID;
    private int amountVertices => mesh.vertexCount;
    private int amountIndices => mesh.triangles.Length;

    private void Awake()
    {
        SetMesh(meshFilter);
    }

    private void OnDisable()
    {
        gpuVertices?.Dispose();
        gpuVertices = null;
        gpuUV?.Dispose();
        gpuUV = null;
        gpuIndices?.Dispose();
        gpuIndices = null;
    }

    private void SetMesh(MeshFilter newMeshFilter)
    {
        meshFilter = newMeshFilter;
        mesh = newMeshFilter.mesh;
        mesh.vertexBufferTarget |= GraphicsBuffer.Target.Structured;
        mesh.indexBufferTarget |= GraphicsBuffer.Target.Structured;

        mesh.SetVertexBufferParams(mesh.vertexCount, 
            new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
            new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, 0),
            new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4, 0),
            new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 1)
        );
    }

    void Update()
    {
        if (shouldCheck)
        {
            if (gpuVertices.IsValid())
            {
                kernelID = shader.FindKernel("FindClosestVertex");
                shader.GetKernelThreadGroupSizes(kernelID, out uint threadGroupSizeX, out _, out _);
                threadGroupSize = Mathf.CeilToInt((float)amountVertices / threadGroupSizeX);

                Vector3 localPos = meshFilter.transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position);
            
                shader.SetBuffer(kernelID, "gpuVertices", gpuVertices);
                shader.SetBuffer(kernelID, "gpuUV", gpuUV);
                //shader.SetBuffer(kernelID, "gpuIndices", gpuIndices);
                shader.SetVector("targetPos", transform.position);
                shader.SetMatrix("localToWorld", meshFilter.transform.localToWorldMatrix);
                shader.SetInt("amountIndices", amountIndices);
                shader.SetInt("amountVertices", amountVertices);
                shader.SetFloat("paintRadius", grassRadius);
                shader.Dispatch(kernelID, threadGroupSize, 1, 1);

                vertex[] vertices = new vertex[amountVertices];
                gpuVertices.GetData(vertices);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            int planetID = collision.gameObject.GetInstanceID();
            if (planetID != cachedPlanetID)
            {
                Vector2[] uvs = new Vector2[amountVertices];
                gpuUV?.GetData(uvs);
                mesh.uv = uvs;
                
                SetMesh(collision.gameObject.GetComponent<MeshFilter>());
                gpuVertices?.Dispose();
                gpuVertices = null;
                gpuUV?.Dispose();
                gpuUV = null;
                gpuIndices?.Dispose();
                gpuIndices = null;
            
                gpuVertices = mesh.GetVertexBuffer(0);
                gpuUV = mesh.GetVertexBuffer(1);
                gpuIndices = mesh.GetIndexBuffer();
                cachedPlanetID = planetID;
            }
        }
    }
    
    // private void OnCollisionExit(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Planet"))
    //     {
    //         Vector2[] uvs = new Vector2[amountVertices];
    //         gpuUV.GetData(uvs);
    //         mesh.uv = uvs;
    //     }
    // }
}

struct planetBuffers
{
    public GraphicsBuffer gpuVertices;
    public GraphicsBuffer gpuUV;
    public GraphicsBuffer gpuIndices;

    public planetBuffers(GraphicsBuffer gpuVertices, GraphicsBuffer gpuUV, GraphicsBuffer gpuIndices)
    {
        this.gpuVertices = gpuVertices;
        this.gpuUV = gpuUV;
        this.gpuIndices = gpuIndices;
    }
}

struct vertex
{
    public Vector3 pos;
    public Vector3 nor;
    public Vector4 tang;
}
