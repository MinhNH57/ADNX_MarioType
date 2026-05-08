using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraReconnect : MonoBehaviour
{
    private CameraFlower cam;

    private void Awake()
    {
        cam = GetComponent<CameraFlower>();
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
        Invoke(nameof(SetupCamera), 0.2f);
    }

    void SetupCamera()
    {
        if (NetworkManager.Singleton == null) return;

        var localPlayer =
            NetworkManager.Singleton.LocalClient.PlayerObject;

        if (localPlayer == null) return;

        cam.SetTarget(localPlayer.transform);

        Debug.Log("Camera đã follow lại player");
    }
}