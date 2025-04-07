using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeachSceneController : MonoBehaviour
{
    private GameObject beachPlane;
    private GameObject waterPlane;
    private float size = 100f; // Add this line to define the size variable
    
    void Start()
    {
        SetupMainCamera();
        CreateBeach();
        CreateWater();
        SetupLighting();
        CreateSkybox();
        CreatePalmTrees(); // Added palm tree generation
    }
    
    private void SetupMainCamera()
    {
	Camera.main.transform.position = new Vector3(0, 30, -20);
	Camera.main.transform.rotation = Quaternion.Euler(45, 0, 0);
    }
    
    private void CreateBeach()
    {
        beachPlane = new GameObject("BeachPlane");
        beachPlane.AddComponent<BeachPlaneGenerator>();
    }
    
    private void CreateWater()
    {
    waterPlane = new GameObject("WaterPlane");
    // Position the water to cover 50% of the area
    waterPlane.transform.position = new Vector3(0, -0.2f, -size/4);
    waterPlane.AddComponent<SimpleWater>();
    }

    private void CreatePalmTrees()
    {
        GameObject palmTreeController = new GameObject("PalmTrees");
        palmTreeController.AddComponent<PalmTreeGenerator>();
    }
    
    private void SetupLighting()
    {
        // Make sure we have a directional light for the sun
        Light[] lights = FindObjectsOfType<Light>();
        Light sunLight = null;
        
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
            {
                sunLight = light;
                break;
            }
        }
        
        if (sunLight == null)
        {
            GameObject sunObject = new GameObject("Sun");
            sunLight = sunObject.AddComponent<Light>();
            sunLight.type = LightType.Directional;
        }
        
        // Setup sun light properties
        sunLight.transform.rotation = Quaternion.Euler(50, -30, 0);
        sunLight.color = new Color(1.0f, 0.95f, 0.85f);
        sunLight.intensity = 1.2f;
        
        // Add a simple sun flare
        sunLight.flare = null; // You would assign a flare asset here if available
    }
    
    private void CreateSkybox()
    {
        // Create a simple gradient skybox
        Material skyboxMaterial = new Material(Shader.Find("Skybox/Procedural"));
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_SunSize", 0.04f);
            skyboxMaterial.SetFloat("_AtmosphereThickness", 1.0f);
            skyboxMaterial.SetColor("_SkyTint", new Color(0.5f, 0.5f, 0.5f));
            skyboxMaterial.SetFloat("_Exposure", 1.3f);
            
            RenderSettings.skybox = skyboxMaterial;
        }
    }
}
