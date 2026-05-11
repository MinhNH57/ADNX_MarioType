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

        bool isServer = false;
        bool isClient = false;

        foreach (string arg in args)
        {
            if (arg == "-client")
            {
                isClient = true;
                break;
            }else if(arg == "-sever")
            {
                isServer = true;
                break;  
            }
        }

        if (isClient)
        {
            Debug.Log("START HOST");
            transport.ConnectionData.Address =
                "127.0.0.1";
            NetworkManager.Singleton.StartClient();
        }
        else if (isServer)
        {
            NetworkManager.Singleton.StartServer();
        }
        else
        {
            NetworkManager.Singleton.StartHost();
        }
    }
}