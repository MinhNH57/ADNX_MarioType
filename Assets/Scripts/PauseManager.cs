using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public GameObject settingsPanel;

    [Header("UI References")]
    public GameObject pauseMenuPanel;

    private bool isPaused = false;

    private void Awake()
    {
        Instance = this;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitToMenu()
    {
        Debug.Log("CurrentPlayerId trước khi quit: " + GameManager.Instance.currentPlayerId);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
}