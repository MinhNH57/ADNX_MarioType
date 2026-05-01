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
        var player = PlayerDataManager.Instance.AddOrGetPlayer(playerName);
        currentPlayerId = player.id;

        Debug.Log("🎯 Current Player: " + player.playerName + " | ID: " + player.id);
    }

    private PlayerData GetCurrentPlayer()
    {
        if (string.IsNullOrEmpty(currentPlayerId))
        {
            Debug.LogError("❌ currentPlayerId chưa được set");
            return null;
        }

        var player = PlayerDataManager.Instance.data.players
            .FirstOrDefault(p => p.id == currentPlayerId);

        if (player == null)
        {
            Debug.LogError("❌ Không tìm thấy player trong data");
        }

        return player;
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

    public void UpdateHighScore(int newScore)
    {
        var player = GetCurrentPlayer();
        if (player == null) return;

        Debug.Log($"🎯 Before Update: {player.highScore}");

        if (newScore > player.highScore)
        {
            player.highScore = newScore;

            PlayerDataManager.Instance.SaveData();
        }
        else
        {
            Debug.Log("⏭️ Score không cao hơn, không update");
        }
    }
}