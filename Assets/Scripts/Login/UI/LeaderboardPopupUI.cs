using UnityEngine;

public class LeaderboardPopupUI : MonoBehaviour
{
    public GameObject popup;
    public LeaderboardUI leaderboardUI;

    public void Open()
    {
        popup.SetActive(true);
        leaderboardUI.ShowLeaderboard();
    }

    public void Close()
    {
        popup.SetActive(false);
    }
}