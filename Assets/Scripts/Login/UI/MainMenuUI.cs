//using UnityEngine;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class MainMenuUI : MonoBehaviour
//{
//    public TMP_InputField nameInput;

//    public void OnClickStart()
//    {
//        string playerName = nameInput.text.Trim();

//        if (string.IsNullOrEmpty(playerName))
//        {
//            Debug.LogWarning("Nhập tên đi bro");
//            return;
//        }

//        var player = PlayerDataManager.Instance.AddOrGetPlayer(playerName);

//        GameManagers.Instance.currentPlayer = player;
//        SceneManager.LoadScene("Game");
//    }
//}

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void OnClickStart()
    {
        string playerName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Nhập tên đi bro");
            return;
        }

        PlayerDataManager.Instance.AddOrGetPlayer(playerName);

        GameManager.Instance.SetCurrentPlayer(playerName);
        SceneManager.LoadScene("ChonNhanVat");
    }
}