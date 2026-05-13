//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;

//    public PlayerData currentPlayer;
//    public int coinCount = 0;
//    public int hightCoint = 0;

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void AddCoin(int amount)
//    {
//        coinCount += amount;
//        hightCoint = coinCount; 
//        if (UIManager.Instance != null)
//        {
//            UIManager.Instance.UpdateCoin(coinCount);
//        }
//    }

//    public void UpdateHighScore(int newScore)
//    {
//        if (currentPlayer == null)
//        {
//            Debug.LogError("❌ currentPlayer NULL");
//            return;
//        }

//        if (newScore > currentPlayer.highScore)
//        {
//            currentPlayer.highScore = newScore;
//            Debug.Log("Update Score :" + currentPlayer.highScore);
//            PlayerDataManager.Instance.SaveData();
//        }
//    }
//}

using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string currentPlayerId;

    public int coinCount = 0;
    public int highCoin = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetCurrentPlayer(string playerName)
    {
        PlayerDataManager.Instance.AddOrGetPlayer(
            playerName,
            (player) =>
            {
                currentPlayerId = player.id.ToString(); 

                Debug.Log(
                    "Current Player ID: " +
                    currentPlayerId
                );
            }
        );
    }

    public void GetCurrentPlayer(Action<PlayerData> callback)
    {
        StartCoroutine(
            PlayerService.GetPlayers(
                (players) =>
                {
                    PlayerData player = players.FirstOrDefault(
                        p => p.id.ToString() == currentPlayerId
                    );
                    callback?.Invoke(player);
                }
            )
        );
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        highCoin = coinCount;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCoin(coinCount);
        }
    }

    //public void UpdateHighScore(int newScore)
    //{
    //    var player = GetCurrentPlayer();
    //    if (player == null) return;

    //    Debug.Log($"🎯 Before Update: {player.hightScore}");

    //    if (newScore > player.hightScore)
    //    {
    //        player.hightScore = newScore;

    //        StartCoroutine(
    //            PlayerService.UpdatePlayer(
    //                player,
    //                onSuccess: (updated) =>
    //                {
    //                    Debug.Log($"✅ Đã lưu high score: {updated.hightScore}");
    //                },
    //                onError: (err) =>
    //                {
    //                    Debug.LogError("[UpdateHighScore] " + err);
    //                }
    //            )
    //        );
    //    }
    //    else
    //    {
    //        Debug.Log("⏭️ Score không cao hơn, không update");
    //    }
    //}

    public void UpdateHighScore(int newScore)
    {
        GetCurrentPlayer((player) =>
        {
            if (player == null) return;

            Debug.Log($"🎯 Before Update: {player.hightScore}");

            if (newScore > player.hightScore)
            {
                player.hightScore = newScore;

                StartCoroutine(
                    PlayerService.UpdatePlayer(
                        player,
                        onSuccess: (updated) =>
                        {
                            Debug.Log($"✅ Đã lưu high score: {updated.hightScore}");
                        },
                        onError: (err) =>
                        {
                            Debug.LogError("[UpdateHighScore] " + err);
                        }
                    )
                );
            }
            else
            {
                Debug.Log("⏭️ Score không cao hơn, không update");
            }
        });
    }

    public void ResetGame()
    {
        coinCount = 0;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCoin(coinCount);
        }
    }
}