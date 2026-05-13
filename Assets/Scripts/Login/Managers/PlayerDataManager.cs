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
    public void AddOrGetPlayer(
        string name,
        System.Action<PlayerData> callback)
    {
        StartCoroutine(
            PlayerService.GetPlayers(
                (players) =>
                {
                    PlayerData existing =
                        players.Find(
                            p => p.playerName == name
                        );

                    if (existing != null)
                    {
                        callback?.Invoke(existing);
                        return;
                    }

                    if (_isCreatingPlayer) return;
                    _isCreatingPlayer = true;

                    PlayerData newPlayer = new PlayerData();
                    newPlayer.playerName = name;
                    newPlayer.hightScore = 0;

                    StartCoroutine(
                        PlayerService.CreatePlayer(
                            newPlayer,
                            onSuccess: (created) =>
                            {
                                _isCreatingPlayer = false;
                                callback?.Invoke(created);
                            },
                            onError: (err) =>
                            {
                                _isCreatingPlayer = false;
                                Debug.LogError("[AddOrGetPlayer] " + err);
                                callback?.Invoke(null);
                            }
                        )
                    );
                }
            )
        );
    }

    private bool _isCreatingPlayer = false; 
    //public void AddOrGetPlayer(
    //    string name,
    //    System.Action<PlayerData> callback)
    //{
    //    StartCoroutine(
    //        PlayerService.GetPlayers(
    //            (players) =>
    //            {
    //                PlayerData existing =
    //                    players.Find(
    //                        p => p.playerName == name
    //                    );
    //                if (existing != null)
    //                {
    //                    callback?.Invoke(existing);
    //                    return;
    //                }

    //                PlayerData newPlayer =
    //                    new PlayerData();

    //                newPlayer.playerName = name;
    //                newPlayer.hightScore = 0;

    //                data.players.Add(newPlayer);

    //                SaveData();

    //                callback?.Invoke(newPlayer);
    //            }
    //        )
    //    );
    //}

    //public void SaveData()
    //{
    //    string json = JsonUtility.ToJson(data, true);
    //    File.WriteAllText(filePath, json);
    //}



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

    public List<PlayerData> GetLeaderboard()
    {
        return data.players
            .OrderByDescending(p => p.hightScore)
            .ToList();
    }
}