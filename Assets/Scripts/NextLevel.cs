using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public AudioManager _audioManager;
    public string _nextScene;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _audioManager.PlaySfx(_audioManager.winClip);
            StartCoroutine(LoadNextScene());
        }
    }
    //IEnumerator LoadNextScene()
    //{
    //    yield return new WaitForSeconds(1.5f);
    //    GameManager.Instance.UpdateHighScore(GameManager.Instance.coinCount);
    //    SceneManager.LoadScene(_nextScene);
    //}

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.UpdateHighScore(GameManager.Instance.coinCount);
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(_nextScene, LoadSceneMode.Single);
        }
    }
}
