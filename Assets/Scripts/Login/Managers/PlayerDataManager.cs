using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    private string filePath;
    public PlayerDataList data = new PlayerDataList();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            filePath = Path.Combine(Application.persistentDataPath, "players.json");

            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PlayerData AddOrGetPlayer(string name)
    {
        var existing = data.players.FirstOrDefault(p => p.playerName == name);

        if (existing != null)
        {
            Debug.Log("Player đã tồn tại");
            return existing;
        }

        PlayerData newPlayer = new PlayerData(name);
        data.players.Add(newPlayer);

        SaveData();

        return newPlayer;
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }


    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            data = JsonUtility.FromJson<PlayerDataList>(json);
            if (data == null)
            {
                data = new PlayerDataList();
            }

            if (data.players == null)
            {
                data.players = new List<PlayerData>();
            }

            Debug.Log("Loaded players: " + data.players.Count);
        }
        else
        {
            data = new PlayerDataList();
            data.players = new List<PlayerData>();
        }
    }
}