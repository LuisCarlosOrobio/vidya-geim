using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PalmTreeGenerator : MonoBehaviour
{
    [SerializeField] private int numTrees = 10;
    [SerializeField] private float spawnRadius = 40f;
    
    void Start()
    {
        for (int i = 0; i < numTrees; i++)
        {
            GeneratePalmTree();
        }
    }
    
    private void GeneratePalmTree()
    {
        // Calculate random position
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(15f, spawnRadius);
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;
        
        // Create parent object
        GameObject palmTree = new GameObject("PalmTree");
        palmTree.transform.position = new Vector3(x, 0, z);
        
        // Create trunk
        GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        trunk.transform.parent = palmTree.transform;
        trunk.transform.localPosition = Vector3.zero;
        trunk.transform.localScale = new Vector3(0.3f, 4f, 0.3f);
        
        // Create trunk material
        Material trunkMaterial = new Material(Shader.Find("Standard"));
        trunkMaterial.SetColor("_Color", new Color(0.5f, 0.35f, 0.2f));
        trunk.GetComponent<Renderer>().material = trunkMaterial;
        
        // Create palm leaves
        int numLeaves = Random.Range(5, 8);
        for (int i = 0; i < numLeaves; i++)
        {
            CreatePalmLeaf(palmTree.transform, i * (360f / numLeaves));
        }
        
        // Apply a slight random rotation to the whole tree
        palmTree.transform.Rotate(0, Random.Range(0, 360), 0);
        
        // Apply a slight random tilt to simulate wind bending
        float tiltAngle = Random.Range(-10f, 10f);
        palmTree.transform.Rotate(tiltAngle, 0, 0);
    }
    
    private void CreatePalmLeaf(Transform parent, float angle)
    {
        GameObject leaf = new GameObject("Leaf");
        leaf.transform.parent = parent;
        leaf.transform.localPosition = new Vector3(0, 3.8f, 0); // Top of trunk
        leaf.transform.localRotation = Quaternion.Euler(40, angle, 0);
        
        MeshFilter meshFilter = leaf.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = leaf.AddComponent<MeshRenderer>();
        
        // Create a simple leaf mesh
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[6];
        int[] triangles = new int[12];
        
        float length = Random.Range(3f, 4f);
        float width = Random.Range(0.2f, 0.4f);
        
        // Center vertex at the base
        vertices[0] = Vector3.zero;
        
        // Tip of the leaf
        vertices[1] = new Vector3(0, 0, length);
        
        // Vertices along the sides (for width)
        vertices[2] = new Vector3(-width, 0, length * 0.3f);
        vertices[3] = new Vector3(width, 0, length * 0.3f);
        vertices[4] = new Vector3(-width * 0.7f, 0, length * 0.6f);
        vertices[5] = new Vector3(width * 0.7f, 0, length * 0.6f);
        
        // Triangles
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 3;
        
        triangles[3] = 2;
        triangles[4] = 4;
        triangles[5] = 3;
        
        triangles[6] = 3;
        triangles[7] = 4;
        triangles[8] = 5;
        
        triangles[9] = 4;
        triangles[10] = 1;
        triangles[11] = 5;
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        meshFilter.mesh = mesh;
        
        // Create leaf material
        Material leafMaterial = new Material(Shader.Find("Standard"));
        leafMaterial.SetColor("_Color", new Color(0.1f, 0.7f, 0.1f));
        meshRenderer.material = leafMaterial;
    }
}
