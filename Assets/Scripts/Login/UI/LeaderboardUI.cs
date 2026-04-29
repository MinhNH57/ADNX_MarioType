//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class LeaderboardUI : MonoBehaviour
//{
//    public Transform content;
//    public GameObject itemPrefab;

//    public void ShowLeaderboard()
//    {
//        if (PlayerDataManager.Instance == null)
//        {
//            Debug.LogError("PlayerDataManager NULL!");
//            return;
//        }

//        var players = PlayerDataManager.Instance.GetLeaderboard();
//        Debug.Log("Số player: " + players.Count);
//        foreach (var p in players)
//            //Debug.Log($"- {p.playerName}: {p.highScore}");

//        foreach (Transform child in content)
//            Destroy(child.gameObject);

//        for (int i = 0; i < players.Count; i++)
//        {
//            var item = Instantiate(itemPrefab, content);
//            item.SetActive(true);
//            var text = item.GetComponentInChildren<Text>(true);
//            Debug.Log("text component: " + (text == null ? "NULL" : "OK"));
//            if (text == null) continue;
//            text.enabled = true;
//            text.text = $"{i + 1}. {players[i].playerName} - {players[i].highScore}";
//            Debug.Log("Set text: " + text.text);
//        }
//    }
//}


using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform content;
    public GameObject itemPrefab;

    public void ShowLeaderboard()
    {
        if (PlayerDataManager.Instance == null)
        {
            Debug.LogError("PlayerDataManager NULL!");
            return;
        }

        var players = PlayerDataManager.Instance.GetLeaderboard();
        Debug.Log("Số player: " + players.Count);


        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count; i++)
        {
            var item = Instantiate(itemPrefab, content);
            item.SetActive(true);

            PlayerRowUI rowUI = item.GetComponent<PlayerRowUI>();

            if (rowUI != null)
            {
                rowUI.SetData(i + 1, players[i].playerName, players[i].highScore);
            }
            else
            {
                Debug.LogError("Prefab chưa được gắn script PlayerRowUI!");
            }
        }
    }
}