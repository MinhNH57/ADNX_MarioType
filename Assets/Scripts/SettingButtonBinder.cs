using UnityEngine;
using UnityEngine.UI;

public class SettingButtonBinder : MonoBehaviour
{
    public enum ButtonType { Open, Close }
    public ButtonType buttonType;

    void Start()
    {
        var button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            var manager = FindObjectOfType<UISettingsManager>();

            if (manager == null)
            {
                Debug.LogError("❌ UISettingsManager NOT FOUND");
                return;
            }

            if (buttonType == ButtonType.Open)
                manager.OpenSetting();
            else
                manager.CloseSetting();
        });
    }
}