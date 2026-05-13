//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Unity.Netcode;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PlayerDeath : NetworkBehaviour
//{
//    public AudioManager audioManager;
//    public GameObject gameOverObject;

//    private void Awake()
//    {
//        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); 
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
//        audioManager.PlaySfx(audioManager.failClip);

//        int finalScore = GameManager.Instance.coinCount;

//        GameManager.Instance.UpdateHighScore(finalScore);

//        gameOverObject.SetActive(true);
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


using System;
using System.Collections;
using System.Collections.Generic;
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
        if (!NetworkManager.Singleton.IsServer)
            return;
        if (!collision.CompareTag("Player"))
            return;

        NetworkObject netWorkObject = collision.gameObject.GetComponent<NetworkObject>();
        if (netWorkObject == null) return;
        if (!netWorkObject.IsSpawned) return; 

        ulong deadClientId = netWorkObject.OwnerClientId;
        ClientRpcParams rpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new List<ulong> { deadClientId }
            }
        };

        ShowGameOverClientRpc(rpcParams);

      
        StartCoroutine(DespawnAfterDelay(netWorkObject));
    }

    private IEnumerator DespawnAfterDelay(NetworkObject netWorkObject)
    {
        yield return new WaitForSeconds(0.2f);

        if (netWorkObject != null && netWorkObject.IsSpawned)
        {
            netWorkObject.Despawn(true);
        }
    }

    [ClientRpc]
    void ShowGameOverClientRpc(
    ClientRpcParams rpcParams = default)
    {
        if (audioManager != null)
        {
            audioManager.PlaySfx(audioManager.failClip);
        }
        if (GameManager.Instance != null)
        {
            int finalScore =
                GameManager.Instance.coinCount;
            GameManager.Instance.UpdateHighScore(finalScore);
        }
        if (gameOverObject != null)
        {
            gameOverObject.SetActive(true);
        }
        StartCoroutine(LoadFail());
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

    IEnumerator LoadFail()
    {
        yield return new WaitForSeconds(1f);
    }
}