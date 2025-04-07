using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeachSandShader : MonoBehaviour
{
    [SerializeField] private Material sandMaterial;
    
    void Start()
    {
        if (sandMaterial == null)
        {
            sandMaterial = new Material(Shader.Find("Standard"));
            GetComponent<Renderer>().material = sandMaterial;
        }
        
        // Create a procedural sand texture
        Texture2D sandTexture = CreateSandTexture(512, 512);
        sandMaterial.mainTexture = sandTexture;
        
        // Set other material properties
        sandMaterial.SetFloat("_Glossiness", 0.1f); // Low glossiness for sand
        sandMaterial.SetColor("_Color", new Color(0.95f, 0.9f, 0.7f, 1f)); // Sandy color
    }
    
    private Texture2D CreateSandTexture(int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Base sand color
                float r = Random.Range(0.85f, 0.95f);
                float g = Random.Range(0.8f, 0.9f);
                float b = Random.Range(0.6f, 0.7f);
                
                // Add noise variation
                float noise = Mathf.PerlinNoise(x * 0.05f, y * 0.05f) * 0.1f;
                
                Color pixelColor = new Color(r + noise, g + noise, b + noise);
                texture.SetPixel(x, y, pixelColor);
            }
        }
        
        texture.Apply();
        return texture;
    }
}
