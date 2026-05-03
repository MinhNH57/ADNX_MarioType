using UnityEngine;

public class UISettingsManager : MonoBehaviour
{
    public GameObject settingPanel;

    //public void OpenSetting()
    //{
    //    settingPanel.SetActive(true);
    //}

    public void OpenSetting()
    {
        Debug.Log("settingPanel is: " + settingPanel);
        if (settingPanel == null)
        {
            // Thử tìm lại
            settingPanel = transform.Find("SettingPanel")?.gameObject;
        }
        settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }
}