//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Unity.Netcode;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PlayerDeath : NetworkBehaviour
//{
//    public AudioManager _audioManager;
//    public GameObject _gameOverObject;

//    private void Awake()
//    {
//        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); 
//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (!IsServer) return;

//        if (collision.gameObject.CompareTag("Player"))
//        {
//            ShowGameOverClientRpc();

//            StartCoroutine(RestartRoutine());
//        }
//    }

//    [ClientRpc]
//    void ShowGameOverClientRpc()
//    {
//        _audioManager.PlaySfx(_audioManager.failClip);

//        int finalScore = GameManager.Instance.coinCount;

//        GameManager.Instance.UpdateHighScore(finalScore);

//        _gameOverObject.SetActive(true);
//    }

//    IEnumerator RestartRoutine()
//    {
//        yield return new WaitForSeconds(2f);

//        NetworkManager.Singleton.Shutdown();

//        while (NetworkManager.Singleton.ShutdownInProgress)
//        {
//            yield return null;
//        }

//        yield return new WaitForSeconds(0.5f);

//        SceneManager.LoadScene("MainMenu");
//    }
//}


using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : NetworkBehaviour
{
    [Header("UI")]
    public GameObject gameOverObject;

    [Header("Audio")]
    public AudioManager audioManager;

    private bool isGameOver = false;

    private void Awake()
    {
        if (audioManager == null)
        {
            GameObject audioObj =
                GameObject.FindGameObjectWithTag("Audio");

            if (audioObj != null)
            {
                audioManager =
                    audioObj.GetComponent<AudioManager>();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // chỉ server xử lý game over
        if (!IsServer) return;

        // tránh gọi nhiều lần
        if (isGameOver) return;

        if (collision.CompareTag("Player"))
        {
            isGameOver = true;

            Debug.Log("GAME OVER");

            // hiện UI cho tất cả client
            ShowGameOverClientRpc();

            // restart game
            StartCoroutine(RestartRoutine());
        }
    }

    [ClientRpc]
    private void ShowGameOverClientRpc()
    {
        Debug.Log("Show Game Over UI");

        // play sound
        if (audioManager != null)
        {
            audioManager.PlaySfx(audioManager.failClip);
        }

        // save high score
        if (GameManager.Instance != null)
        {
            int finalScore =
                GameManager.Instance.coinCount;

            GameManager.Instance.UpdateHighScore(finalScore);
        }

        // show game over ui
        if (gameOverObject != null)
        {
            gameOverObject.SetActive(true);
        }
    }

    private IEnumerator RestartRoutine()
    {
        yield return new WaitForSeconds(2f);

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();

            while (NetworkManager.Singleton.ShutdownInProgress)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            Destroy(NetworkManager.Singleton.gameObject);
        }

        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}