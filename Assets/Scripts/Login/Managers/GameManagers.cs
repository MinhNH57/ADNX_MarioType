using UnityEngine;

public class GameManagers : MonoBehaviour
{
    public static GameManagers Instance;

    public PlayerData currentPlayer;

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
}