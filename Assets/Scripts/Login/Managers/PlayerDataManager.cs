using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

    public void LoadData()
    {
        StartCoroutine(
            PlayerService.GetPlayers(
                (players) =>
                {
                    data.players = players;

                    Debug.Log("Hello " +data.players.Count());
                }
            )
        );
    }

    public List<PlayerData> GetLeaderboard()
    {
        var _lst = data.players.OrderByDescending(p => p.hightScore).ToList();
        return _lst;
    }
}