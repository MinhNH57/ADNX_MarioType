using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviour
{
    void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("Không tìm thấy NetworkManager");
            return;
        }

        var transport =
            NetworkManager.Singleton.GetComponent<UnityTransport>();

        string[] args =
            System.Environment.GetCommandLineArgs();

        bool isHost = false;

        foreach (string arg in args)
        {
            if (arg == "-host")
            {
                isHost = true;
                break;
            }
        }

        if (isHost)
        {
            Debug.Log("START HOST");

            NetworkManager.Singleton.StartHost();
        }
        else
        {
            Debug.Log("START CLIENT");

            transport.ConnectionData.Address =
                "127.0.0.1";

            NetworkManager.Singleton.StartClient();
        }
    }
}