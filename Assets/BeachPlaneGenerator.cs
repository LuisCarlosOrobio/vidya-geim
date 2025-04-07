using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeachPlaneGenerator : MonoBehaviour
{
    [SerializeField] private int resolution = 100; // Grid size
    [SerializeField] private float size = 100f; // Overall size
    [SerializeField] private float noiseScale = 0.3f; // Controls general height variation
    [SerializeField] private float heightScale = 3.0f; // Max height of terrain
    [SerializeField] private Material beachMaterial;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    void Start()
    {
        InitializeComponents();
        GenerateBeachMesh();
    }

    private void InitializeComponents()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        
        // Create a sandy material
        Material sandMaterial = new Material(Shader.Find("Standard"));
        sandMaterial.SetColor("_Color", new Color(0.95f, 0.85f, 0.55f)); // Sandy yellow color
        sandMaterial.SetFloat("_Glossiness", 0.1f); // Low glossiness for sand
        
        meshRenderer.material = sandMaterial;
    }

    private void GenerateBeachMesh()
    {
        Mesh mesh = new Mesh();
        
        // Create vertices, triangles, UVs
        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        int[] triangles = new int[resolution * resolution * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        // Generate vertices
for (int z = 0; z <= resolution; z++)
{
    for (int x = 0; x <= resolution; x++)
    {
        int i = z * (resolution + 1) + x;
        float px = x * size / resolution - size / 2;
        float pz = z * size / resolution - size / 2;
        
        // Use Perlin noise for height
        float py = Mathf.PerlinNoise(x * noiseScale, z * noiseScale) * heightScale;
        
        // Create a more natural shoreline transition at the middle (50%)
        float normalizedZ = (float)z / resolution;
        float waterSlope = 1.0f;
        
        if (normalizedZ > 0.45f && normalizedZ < 0.55f) {
            // Create a gradual slope at the shoreline
            waterSlope = Mathf.Lerp(1.0f, 0.2f, (normalizedZ - 0.45f) / 0.1f);
        }
        else if (normalizedZ >= 0.55f) {
            // Keep water area low
            waterSlope = 0.2f;
        }
        
        py *= waterSlope;
        
        vertices[i] = new Vector3(px, py, pz);
        uvs[i] = new Vector2((float)x / resolution, (float)z / resolution);
    }
}

        // Generate triangles
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

        // Assign to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}
