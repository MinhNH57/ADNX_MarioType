using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAutoRegister : MonoBehaviour
{
    private Light2D myLight;

    void Awake()
    {
        myLight = GetComponent<Light2D>();
    }

    void OnEnable()
    {
        if (LightManager.Instance != null)
        {
            LightManager.Instance.RegisterLight(myLight);
        }
    }
}