using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public void OnToggleChanged(bool isOn)
    {
        if (LightManager.Instance == null)
        {
            Debug.Log("LightManager.Instance is NULL");
            return;
        }
        LightManager.Instance.SetLight(isOn);
    }
}