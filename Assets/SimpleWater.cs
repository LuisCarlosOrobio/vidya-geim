using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleWater : MonoBehaviour
{
    [SerializeField] private int resolution = 50;
    [SerializeField] private float size = 100f;
    [SerializeField] private float waveHeight = 0.2f;
    [SerializeField] private float waveFrequency = 0.5f;
    [SerializeField] private float waveSpeed = 1.0f;
    
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Vector3[] baseVertices;
    private Vector3[] vertices;
    
    void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        
        // Basic water material
        Material waterMaterial = new Material(Shader.Find("Standard"));
        waterMaterial.SetColor("_Color", new Color(0.1f, 0.5f, 0.8f, 0.7f));
        waterMaterial.SetFloat("_Glossiness", 0.9f);
        
        // Enable transparency
        waterMaterial.SetFloat("_Mode", 3); // Transparent mode
        waterMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        waterMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        waterMaterial.SetInt("_ZWrite", 0);
        waterMaterial.DisableKeyword("_ALPHATEST_ON");
        waterMaterial.EnableKeyword("_ALPHABLEND_ON");
        waterMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        waterMaterial.renderQueue = 3000;
        
        meshRenderer.material = waterMaterial;
        
        GenerateWaterMesh();
    }
    
    void Update()
    {
        AnimateWaves();
    }
    
    private void GenerateWaterMesh()
    {
       
	Mesh mesh = new Mesh();
    
    // Create vertices, triangles, UVs
    vertices = new Vector3[(resolution + 1) * (resolution + 1)];
    baseVertices = new Vector3[vertices.Length];
    int[] triangles = new int[resolution * resolution * 6];
    Vector2[] uvs = new Vector2[vertices.Length];
    
    for (int z = 0; z <= resolution; z++)
    {
        for (int x = 0; x <= resolution; x++)
        {
            int i = z * (resolution + 1) + x;
            
            // Make water cover half the area - start from middle of the area and go forward
            float px = x * size / resolution - size / 2;
            float pz = (z * size / resolution) - (size / 4); // This shifts the starting point backward
            
            vertices[i] = new Vector3(px, 0, pz);
            baseVertices[i] = vertices[i];
            uvs[i] = new Vector2((float)x / resolution, (float)z / resolution);
        }
    }
 
        int tris = 0;
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = z * (resolution + 1) + x;
                
                triangles[tris + 0] = i;
                triangles[tris + 1] = i + resolution + 1;
                triangles[tris + 2] = i + 1;
                
                triangles[tris + 3] = i + 1;
                triangles[tris + 4] = i + resolution + 1;
                triangles[tris + 5] = i + resolution + 2;
                
                tris += 6;
            }
        }
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
    
    private void AnimateWaves()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];
            
            // Create wave effect
            float dx = vertex.x * waveFrequency;
            float dz = vertex.z * waveFrequency;
            float time = Time.time * waveSpeed;
            
            float y = Mathf.Sin(dx + time) * waveHeight + 
                     Mathf.Sin(dz + time) * waveHeight;
            
            // Make waves smaller near shore
            if (vertex.z < 10f) {
                y *= vertex.z / 10f; // Reduce wave height near shore
            }
            
            vertices[i] = new Vector3(vertex.x, y, vertex.z);
        }
        
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
