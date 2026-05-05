using System;
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

    public void OnToggleChanged(bool value)
    {
        if (LightManager.Instance == null) return;
        Debug.Log("OnToggleChanged" + toggleLight.isOn + " " + DateTime.Now.ToString());
        value = toggleLight.isOn;
        LightManager.Instance.SetLight(value);
    }
}