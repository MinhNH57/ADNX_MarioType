using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneSpawn : NetworkBehaviour
{
    private NetworkTransform netTransform;

    private void Awake()
    {
        netTransform = GetComponent<NetworkTransform>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!IsServer) return;
        StartCoroutine(SetSpawnPosition(scene));
    }

    IEnumerator SetSpawnPosition(Scene scene)
    {
        yield return null;
        yield return null;

        Vector3 spawnPos = Vector3.zero;
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawnPoint != null)
        {
            spawnPos = spawnPoint.transform.position;
        }

        // Gọi ClientRpc để đúng owner tự teleport
        TeleportClientRpc(spawnPos);
    }

    [ClientRpc]
    private void TeleportClientRpc(Vector3 spawnPos)
    {
        // Chỉ chạy trên owner của object này
        if (!IsOwner) return;

        netTransform.Teleport(
            spawnPos,
            Quaternion.identity,
            transform.localScale
        );
        Debug.Log("Teleport player tới: " + spawnPos);
    }
}