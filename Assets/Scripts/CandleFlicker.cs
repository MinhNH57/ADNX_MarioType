using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleFlicker : MonoBehaviour
{
    public Light2D light2D;

    [Header("Intensity")]
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;

    [Header("Radius")]
    public float minRadius = 1.8f;
    public float maxRadius = 2.2f;

    [Header("Speed")]
    public float flickerSpeed = 3f;

    private float noiseOffset;

    void Start()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();

        noiseOffset = Mathf.PerlinNoise(0f, 100f);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, noiseOffset);

        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        light2D.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, noise);
    }
}
