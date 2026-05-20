using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : NetworkBehaviour
{
    public AudioManager _audioManager;
    public string _nextScene;
    private bool _isLoading = false;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio")
                            .GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (_isLoading) return;

        _isLoading = true;
        _audioManager.PlaySfx(_audioManager.winClip);
        StartCoroutine(NotifyServer ());
    }

    IEnumerator NotifyServer()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.UpdateHighScore(GameManager.Instance.coinCount);
        RequestLoadSceneServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestLoadSceneServerRpc()
    {
        StartCoroutine(DelayedLoad());
    }

    IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(0.5f);
        NetworkManager.Singleton.SceneManager.LoadScene(
            _nextScene, LoadSceneMode.Single);
    }
}