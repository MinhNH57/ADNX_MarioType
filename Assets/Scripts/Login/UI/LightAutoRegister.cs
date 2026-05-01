using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAutoRegister : MonoBehaviour
{
    private Light2D light2D;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void OnEnable()
    {
        if (LightManager.Instance != null)
            LightManager.Instance.RegisterLight(light2D);
    }

    void OnDisable()
    {
        if (LightManager.Instance != null)
            LightManager.Instance.UnregisterLight(light2D);
    }
}