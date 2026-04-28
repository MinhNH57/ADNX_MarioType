using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI coinText;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = "Coin: " + coin;
    }
}
