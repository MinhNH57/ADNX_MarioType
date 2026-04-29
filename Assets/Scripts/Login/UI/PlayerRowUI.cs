using UnityEngine;
using TMPro; 

public class PlayerRowUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    public void SetData(int rank, string playerName, int score)
    {
        rankText.text = rank.ToString();
        nameText.text = playerName;
        scoreText.text = score.ToString();

        if (rank == 1)
        {
            rankText.color = Color.yellow;
            nameText.color = Color.yellow;
            scoreText.color = Color.yellow;
        }
        else if (rank == 2)
        {
            rankText.color = new Color(0.9f, 0.9f, 0.9f);
            nameText.color = new Color(0.9f, 0.9f, 0.9f);
            scoreText.color = new Color(0.9f, 0.9f, 0.9f);
        }
        else if (rank == 3)
        {
            rankText.color = new Color(0.8f, 0.5f, 0.2f);
            nameText.color = new Color(0.8f, 0.5f, 0.2f);
            scoreText.color = new Color(0.8f, 0.5f, 0.2f);
        }
        else
        {
            rankText.color = Color.white;
        }
    }
}