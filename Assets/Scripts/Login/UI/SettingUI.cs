using System;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingUI : MonoBehaviour
{
    //public void OnToggleChanged(bool value)
    //{
    //    if (LightManager.Instance == null)
    //    {
    //        return;
    //    }
    //    LightManager.Instance.SetLight(value);
    //}
    public static SettingUI Instances;
    [SerializeField] private Toggle toggleLight;
    [SerializeField] private Toggle toggleSounds;
    public bool? IsOnSound;

    private void Awake()
    {
        Instances = GetComponent<SettingUI>();  
    }
    public void OnToggleChanged(bool value)
    {
        if (LightManager.Instance == null) return;
        value = toggleLight.isOn;
        LightManager.Instance.SetLight(value);
    }

    public void OnToggleChangedSound(bool value)
    {
        value = toggleSounds.isOn;
        IsOnSound = value;
        if(value)
        {
            AudioListener.pause = false;
        }
        else
        {
            AudioListener.pause = true;
        }
    }
}