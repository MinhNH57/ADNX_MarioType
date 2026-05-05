//using System;
//using System.Net.WebSockets;
//using UnityEngine;
//using UnityEngine.UI;
//using static UnityEngine.Rendering.DebugUI;

//public class SettingUI : MonoBehaviour
//{
//    //public void OnToggleChanged(bool value)
//    //{
//    //    if (LightManager.Instance == null)
//    //    {
//    //        return;
//    //    }
//    //    LightManager.Instance.SetLight(value);
//    //}
//    public static SettingUI Instances;
//    [SerializeField] private Toggle toggleLight;
//    [SerializeField] private Toggle toggleSounds;
//    public bool? IsOnSound;

//    private void Awake()
//    {
//        Instances = GetComponent<SettingUI>();  
//    }
//    public void OnToggleChanged(bool value)
//    {
//        if (LightManager.Instance == null) return;
//        value = toggleLight.isOn;
//        LightManager.Instance.SetLight(value);
//    }

//    public void OnToggleChangedSound(bool value)
//    {
//        value = toggleSounds.isOn;
//        IsOnSound = value;
//        if(value)
//        {
//            AudioListener.pause = false;
//        }
//        else
//        {
//            AudioListener.pause = true;
//        }
//    }
//}

using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public static SettingUI Instances;

    [SerializeField] private Toggle toggleLight;
    [SerializeField] private Toggle toggleSounds;

    private void Awake()
    {
        Instances = this;
    }

    private void Start()
    {
        bool isOn = PlayerPrefs.GetInt("SOUND", 1) == 1;
        bool isLightOn = LightManager.Instance.isLightOn;

        toggleSounds.isOn = isOn;
        toggleLight.isOn = isLightOn;
        AudioListener.pause = !isOn;

        toggleSounds.onValueChanged.AddListener(OnToggleChangedSound);
    }

    public void OnToggleChanged(bool value)
    {
        if (LightManager.Instance == null) return;
        value = toggleLight.isOn;
        LightManager.Instance.SetLight(value);
    }

    public void OnToggleChangedSound(bool value)
    {
        PlayerPrefs.SetInt("SOUND", value ? 1 : 0);

        AudioListener.pause = !value;

        Debug.Log("Sound: " + value);
    }
}