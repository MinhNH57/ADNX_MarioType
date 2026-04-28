using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string id;
    public string playerName;
    public int highScore;

    public PlayerData(string name)
    {
        id = System.Guid.NewGuid().ToString();
        playerName = name;
        highScore = 0;
    }
}
