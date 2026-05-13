//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class SawRotate : MonoBehaviour
//{
//    public AudioManager _audioManager;
//    public float rotateSpeed = 200f;
//    public GameObject _gameOverObject;
//    private void Awake()
//    {
//        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
//    }
//    void Update()
//    {
//        foreach(Transform child in transform)
//        {
//            child.Rotate(0, 0, rotateSpeed * Time.deltaTime);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if(collision.CompareTag("Player"))
//        {
//            _audioManager.PlaySfx(_audioManager.failClip);
//            int finalScore = GameManager.Instance.coinCount;
//            Debug.Log("SawRotate" + finalScore);
//            GameManager.Instance.UpdateHighScore(finalScore);
//            StartCoroutine(LoadFail());
//            collision.gameObject.SetActive(false);
//            Debug.Log("SawRotate");
//            _gameOverObject.SetActive(true);
//        }
//    }

//    IEnumerator LoadFail()
//    {
//        yield return new WaitForSeconds(1f);
//    }
//}


using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SawRotate : NetworkBehaviour
{
    public AudioManager _audioManager;
    public float rotateSpeed = 200f;
    public GameObject _gameOverObject;

    private bool isTriggered = false; 

    private void Awake()
    {
        var audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null)
        {
            _audioManager = audioObj.GetComponent<AudioManager>();
        }
        else
        {
            Debug.LogError("❌ Không tìm thấy AudioManager");
        }
    }

    void Update()
    {
        foreach (Transform child in transform)
        {
            child.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        if (!collision.CompareTag("Player")) return;

        NetworkObject netWorkObject = collision.gameObject.GetComponent<NetworkObject>();

        if (netWorkObject == null) return;
        if (!netWorkObject.IsSpawned) return; // ← thêm dòng này

        ulong deadClientId = netWorkObject.OwnerClientId;
        ClientRpcParams rpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new List<ulong> { deadClientId }
            }
        };

        ShowGameOverClientRpc(rpcParams);
        netWorkObject.Despawn(true);
    }

    [ClientRpc]
    void ShowGameOverClientRpc(
    ClientRpcParams rpcParams = default)
    {
        if (_audioManager != null)
        {
            _audioManager.PlaySfx(_audioManager.failClip);
        }
        if (GameManager.Instance != null)
        {
            int finalScore =
                GameManager.Instance.coinCount;
            GameManager.Instance.UpdateHighScore(finalScore);
        }
        if (_gameOverObject != null)
        {
            _gameOverObject.SetActive(true);
        }
        StartCoroutine(LoadFail());
    }

    IEnumerator LoadFail()
    {
        yield return new WaitForSeconds(1f);
    }
}

//isTriggered = true;

//if (_audioManager != null)
//{
//    _audioManager.PlaySfx(_audioManager.failClip);
//}

//int finalScore = GameManager.Instance.coinCount;
//Debug.Log("💀 Player chết | Score: " + finalScore);

//if (GameManager.Instance != null)
//{
//    GameManager.Instance.UpdateHighScore(finalScore);
//}
//else
//{
//    Debug.LogError("❌ GameManager NULL");
//}

//collision.gameObject.SetActive(false);

//if (_gameOverObject != null)
//{
//    _gameOverObject.SetActive(true);
//}
//else
//{
//    Debug.LogError("❌ GameOverObject chưa gán");
//}

//StartCoroutine(LoadFail());