
using System;
using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class AutoLANNetcode : MonoBehaviour
{
    private bool foundHost = false;

    IEnumerator Start()
    {
        LANDiscovery.Instance.OnHostFound += OnHostFound;

        LANDiscovery.Instance.StartListening();

        yield return new WaitForSeconds(3f);

        if (NetworkManager.Singleton.IsClient ||
            NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Network đã chạy -> không StartHost");
            NetworkManager.Singleton.Shutdown();
            yield return new WaitForSeconds(0.2f);
            NetworkManager.Singleton.StartHost();
            yield break;
        }
        foundHost = NetworkManager.Singleton;
        if (!foundHost)
        {
            Debug.Log("Không thấy Host -> StartHost");

            NetworkManager.Singleton.StartHost();

            LANDiscovery.Instance.StartBroadcast();
        }
    }

    void OnHostFound(string ip)
    {
        if (foundHost) return;
        if (NetworkManager.Singleton.IsClient ||
            NetworkManager.Singleton.IsServer)
        {
            return;
        }

        foundHost = true;

        Debug.Log("Join Host: " + ip);

        var transport =
            NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.ConnectionData.Address = ip;

        NetworkManager.Singleton.StartClient();
    }
}