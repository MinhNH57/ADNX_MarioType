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
    [SerializeField] private Toggle toggleLight;
    [SerializeField] private Toggle toggleSounds;


    public void OnToggleChanged(bool value)
    {
        if (LightManager.Instance == null) return;
        value = toggleLight.isOn;
        LightManager.Instance.SetLight(value);
    }

    public void OnToggleChangedSound(bool value)
    {
        value = toggleSounds.isOn;
        Debug.Log("Sounds :" + value.ToString());
    }
}