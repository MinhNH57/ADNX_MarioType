using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    public bool isLightOn;
    public float brightness = 1f;

    private List<Light2D> lights = new List<Light2D>();

    public void RegisterLight(Light2D light)
    {
        if (light == null) return;
        if (!lights.Contains(light))
        {
            Debug.Log("Đèn đăngt ký " + light.name);
            lights.Add(light);
            ApplyToLight(light);
        }
    }
    private void Awake()
    {

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

    //void ApplyToLight(Light2D light)
    //{
    //    if (isLightOn)
    //    {
    //        light.enabled = true;
    //        light.intensity = brightness;
    //    }
    //    else
    //    {
    //        light.enabled = false;
    //        light.intensity = 0f;
    //    }
    //}

    void ApplyToLight(Light2D light)
    {
        if (light == null) return;
        Debug.Log(isLightOn + DateTime.Now.ToString());
        if (isLightOn)
        {
            light.intensity = 1;
            Debug.Log("M");
        }
        else
        {
            light.intensity = 0;
            Debug.Log("I");
        }
    }

    // Thêm hàm này để gọi mỗi khi load scene mới hoặc reset
    public void ClearDeadLights()
    {
        lights.RemoveAll(l => l == null);
    }
}