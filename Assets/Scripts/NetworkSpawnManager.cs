using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSpawnManager : MonoBehaviour
{
    public Transform transformPlayer;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitAndSetPosition(scene));
    }

    IEnumerator WaitAndSetPosition(Scene scene)
    {
        yield return new WaitForSeconds(0.5f);

        // Debug kiểm tra
        Debug.Log("transformPlayer null? " + (transformPlayer == null));
        if (transformPlayer != null)
            Debug.Log("transformPlayer active? " + transformPlayer.gameObject.activeInHierarchy);

        var spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        Debug.Log("SpawnPoint tìm được? " + (spawnPoint != null));
        if (spawnPoint != null)
            Debug.Log("SpawnPoint pos: " + spawnPoint.transform.position);

        var positionCam = Camera.main.transform.position;
        Vector3 spawnPos = spawnPoint != null
            ? spawnPoint.transform.position
            : new Vector3(positionCam.x, positionCam.y, 0);
        if (scene.name == "GameLevel2")
        {
            transformPlayer.position = new Vector3(-5.06f, 0.05f, 0);
        }
        else
        {
            transformPlayer.position = spawnPos;
        }
        Debug.Log("Đã set Player tại: " + spawnPos);
        Debug.Log("Player thực tế ở: " + transformPlayer.position);
    }

}