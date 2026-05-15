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