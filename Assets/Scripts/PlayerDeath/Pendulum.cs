////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;

////public class NewBehaviourScript : MonoBehaviour
////{
////    public GameObject _gameOverObject;
////    public float speed = 2f; 
////    public float angle = 45f;   
////    private bool isTriggered = false;
////    public AudioManager _audioManager;

////    void Update()
////    {
////        float z = Mathf.Sin(Time.time * speed) * angle;
////        transform.rotation = Quaternion.Euler(0, 0, z);
////    }

////    //private void OnTriggerEnter2D(Collider2D collision)
////    //{
////    //    if (collision.gameObject.CompareTag("Player"))
////    //    {
////    //        collision.gameObject.SetActive(false);
////    //        if(_gameOverObject != null)
////    //        {
////    //            _gameOverObject.SetActive(true);
////    //        }
////    //        else
////    //        {
////    //            Debug.Log("_gameOverObject is null");
////    //        }
////    //    }
////    //}

////    private void OnTriggerEnter2D(Collider2D collision)
////    {
////        if (isTriggered) return;
////        if (!collision.CompareTag("Player")) return;

////        isTriggered = true;

////        if (_audioManager != null)
////        {
////            _audioManager.PlaySfx(_audioManager.failClip);
////        }

////        int finalScore = GameManager.Instance.coinCount;
////        Debug.Log("💀 Player chết | Score: " + finalScore);

////        if (GameManager.Instance != null)
////        {
////            GameManager.Instance.UpdateHighScore(finalScore);
////        }
////        else
////        {
////            Debug.LogError("❌ GameManager NULL");
////        }

////        collision.gameObject.SetActive(false);

////        if (_gameOverObject != null)
////        {
////            _gameOverObject.SetActive(true);
////        }
////        else
////        {
////            Debug.LogError("❌ GameOverObject chưa gán");
////        }

////        StartCoroutine(LoadFail());
////    }

////    IEnumerator LoadFail()
////    {
////        yield return new WaitForSeconds(1f);
////    }
////}


//using System.Collections;
//using System.Collections.Generic;
//using Unity.Netcode;
//using UnityEngine;
//public class Pendulum : NetworkBehaviour
//{
//    [Header("UI")]
//    public GameObject _gameOverObject;
//    [Header("Animation")]
//    public float speed = 2f;
//    public float angle = 45f;
//    [Header("Audio")]
//    public AudioManager _audioManager;
//    void Update()
//    {
//        float z =
//            Mathf.Sin(Time.time * speed) * angle;
//        transform.rotation =
//            Quaternion.Euler(0, 0, z);
//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (!NetworkManager.Singleton.IsServer)
//            return;
//        if (!collision.CompareTag("Player"))
//            return;
//        NetworkObject playerNetObj =
//            collision.GetComponent<NetworkObject>();
//        if (playerNetObj == null)
//            return;
//        if (!canKill) return;
//        ulong deadClientId =
//            playerNetObj.OwnerClientId;
//        ClientRpcParams rpcParams = new ClientRpcParams
//        {
//            Send = new ClientRpcSendParams
//            {
//                TargetClientIds = new List<ulong> { deadClientId }
//            }
//        };
//        ShowGameOverClientRpc(rpcParams);
//        _gameOverObject.SetActive(true);
//        playerNetObj.Despawn(true);
//    }
//    private bool canKill = false;

//    IEnumerator Start()
//    {
//        yield return new WaitForSeconds(1f);
//        canKill = true;
//    }
//    [ClientRpc]
//    void ShowGameOverClientRpc(
//        ClientRpcParams rpcParams = default)
//    {
//        if (_audioManager != null)
//        {
//            _audioManager.PlaySfx(_audioManager.failClip);
//        }
//        if (GameManager.Instance != null)
//        {
//            int finalScore =
//                GameManager.Instance.coinCount;
//            GameManager.Instance.UpdateHighScore(finalScore);
//        }
//        StartCoroutine(LoadFail());
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

public class Pendulum : NetworkBehaviour
{
    [Header("UI")]
    public GameObject _gameOverObject;
    [Header("Animation")]
    public float speed = 2f;
    public float angle = 45f;
    [Header("Audio")]
    public AudioManager _audioManager;
    private bool canKill = false;

    void Update()
    {
        float z = Mathf.Sin(Time.time * speed) * angle;
        transform.rotation = Quaternion.Euler(0, 0, z);
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        canKill = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canKill) return;
        if (!collision.CompareTag("Player")) return;

        NetworkObject playerNetObj = collision.GetComponent<NetworkObject>();
        if (playerNetObj == null) return;

        // Nếu là Server → xử lý luôn
        if (NetworkManager.Singleton.IsServer)
        {
            HandleDeath(playerNetObj.OwnerClientId);
        }
        // Nếu là Client → báo lên Server
        else if (playerNetObj.IsOwner)
        {
            NotifyDeathServerRpc(playerNetObj.OwnerClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyDeathServerRpc(ulong deadClientId)
    {
        HandleDeath(deadClientId);
    }

    private void HandleDeath(ulong deadClientId)
    {
        // Hiện GameOver cho người chết
        ClientRpcParams rpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new List<ulong> { deadClientId }
            }
        };
        ShowGameOverClientRpc(rpcParams);

        // Hiện GameOver cho Host
        if (_gameOverObject != null)
            _gameOverObject.SetActive(true);

        // Despawn player chết
        if (NetworkManager.Singleton.ConnectedClients
            .TryGetValue(deadClientId, out var client))
        {
            NetworkObject netObj = client.PlayerObject;
            if (netObj != null && netObj.IsSpawned)
                netObj.Despawn(true);
        }
    }

    [ClientRpc]
    void ShowGameOverClientRpc(ClientRpcParams rpcParams = default)
    {
        if (_audioManager != null)
            _audioManager.PlaySfx(_audioManager.failClip);
        if (GameManager.Instance != null)
            GameManager.Instance.UpdateHighScore(GameManager.Instance.coinCount);
        if (_gameOverObject != null)
            _gameOverObject.SetActive(true);
        StartCoroutine(LoadFail());
    }

    IEnumerator LoadFail()
    {
        yield return new WaitForSeconds(1f);
    }
}