using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    public bool isLightOn = true;
    public float brightness = 1f;

    private List<Light2D> lights = new List<Light2D>();

    public void RegisterLight(Light2D light)
    {
        lights.Add(light);
        Debug.Log("Register: " + light.name);

        ApplyToLight(light);
    }
    private void Awake()
    {
        Debug.Log("LightManager Awake in scene: " + gameObject.scene.name);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnregisterLight(Light2D light)
    {
        lights.Remove(light);
    }

    public void SetLight(bool on)
    {
        Debug.Log("LightManager:" + on);
        isLightOn = on;
        UpdateAll();
    }

    public void SetBrightness(float value)
    {
        brightness = value;
        UpdateAll();
    }

    void UpdateAll()
    {
        foreach (var light in lights)
        {
            ApplyToLight(light);
        }
    }

    void ApplyToLight(Light2D light)
    {
        Debug.Log("isLightOn" + isLightOn);
        light.enabled = isLightOn;
        if (isLightOn)
        {
            light.intensity = 1;
        }
        else
        {
            light.intensity = 0;
        }
    }
}