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

        // bắt đầu lắng nghe host
        LANDiscovery.Instance.StartListening();

        Debug.Log("Đang tìm Host LAN...");

        yield return new WaitForSeconds(3f);

        // không thấy ai -> làm host
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

        foundHost = true;

        Debug.Log("Join Host: " + ip);

        var transport =
            NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.ConnectionData.Address = ip;

        NetworkManager.Singleton.StartClient();
    }
}