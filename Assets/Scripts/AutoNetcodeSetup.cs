using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class AutoNetcodeSetup : MonoBehaviour
{
    private bool connected = false;

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("Không tìm thấy NetworkManager!");
            return;
        }

        if (NetworkManager.Singleton.IsClient ||
            NetworkManager.Singleton.IsServer)
        {
            return;
        }

        StartCoroutine(AutoConnectRoutine());
    }

    IEnumerator AutoConnectRoutine()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("Đang thử kết nối Host...");

        NetworkManager.Singleton.OnClientConnectedCallback += OnConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;

        NetworkManager.Singleton.StartClient();

        float timeout = 5f;
        float timer = 0f;

        while (timer < timeout && !connected)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Nếu connect được -> thành client
        if (connected)
        {
            Debug.Log("Join thành công với tư cách Client.");
            yield break;
        }

        // Không connect được -> Host
        Debug.Log("Không tìm thấy Host -> tạo Host mới");

        NetworkManager.Singleton.Shutdown();

        yield return new WaitUntil(
            () => !NetworkManager.Singleton.ShutdownInProgress
        );

        yield return new WaitForSeconds(1f);

        bool success =
            NetworkManager.Singleton.StartHost();

        Debug.Log("StartHost: " + success);
    }

    private void OnConnected(ulong clientId)
    {
        if (clientId ==
            NetworkManager.Singleton.LocalClientId)
        {
            connected = true;
        }
    }

    private void OnDisconnected(ulong clientId)
    {
        Debug.Log("Disconnect: " + clientId);
    }
}