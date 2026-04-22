using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coinCount = 0;

    void Awake()
    {
        Instance = this;
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;

        UIManager.Instance.UpdateCoin(coinCount);
    }
}
