using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingsManager : MonoBehaviour
{
    public GameObject settingPanel;

    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }
}
