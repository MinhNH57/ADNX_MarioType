using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public static class PlayerService
{
    public static IEnumerator GetPlayers(
        Action<List<PlayerData>> callback)
    {
        yield return ApiService.Get(
            "Unity/Player",
            (json) =>
            {
                List<PlayerData> players =
                    JsonConvert.DeserializeObject
                    <List<PlayerData>>(json);

                callback?.Invoke(players);
            }
        );
    }

    public static IEnumerator CreatePlayer(
    PlayerData player,
    Action<PlayerData> onSuccess,
    Action<string> onError = null)
    {
        string json = JsonConvert.SerializeObject(player);
        Debug.Log("[CreatePlayer] JSON gửi lên: " + json);
        yield return ApiService.Post(
            "Unity/AddPlayer",
            json,
            (responseJson) =>
            {
                PlayerData created =
                    JsonConvert.DeserializeObject
                    <PlayerData>(responseJson);
                onSuccess?.Invoke(created);
            },
            onError
        );
    }

    public static IEnumerator UpdatePlayer(
    PlayerData player,
    Action<PlayerData> onSuccess = null,
    Action<string> onError = null)
    {
        string json = JsonConvert.SerializeObject(player);
        Debug.Log(player.id);
        yield return ApiService.Put(
            "Unity/UpdatePlayer",
            json,
            (responseJson) =>
            {
                Debug.Log("Response: " + responseJson);
                PlayerData updated =
                    JsonConvert.DeserializeObject
                    <PlayerData>(responseJson);
                onSuccess?.Invoke(updated);
            },
            onError
        );
    }
}