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
        if (settingPanel == null)
        {
            settingPanel = transform.Find("SettingPanel")?.gameObject;
        }
        Debug.Log(transform.Find("SettingPanel"));
        settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }
}