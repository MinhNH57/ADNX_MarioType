using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class LANDiscovery : MonoBehaviour
{
    public static LANDiscovery Instance;

    public int port = 7777;

    private UdpClient udpClient;

    public Action<string> OnHostFound;

    private void Awake()
    {
        Instance = this;
    }

    // HOST gọi
    public void StartBroadcast()
    {
        udpClient = new UdpClient();

        udpClient.EnableBroadcast = true;

        InvokeRepeating(nameof(BroadcastHost), 1f, 1f);
    }

    void BroadcastHost()
    {
        try
        {
            string message = "UNITY_HOST";

            byte[] data =
                Encoding.UTF8.GetBytes(message);

            IPEndPoint endPoint =
                new IPEndPoint(IPAddress.Broadcast, port);

            udpClient.Send(data, data.Length, endPoint);

            Debug.Log("Broadcast HOST");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    // CLIENT gọi
    public void StartListening()
    {
        udpClient = new UdpClient(port);

        udpClient.BeginReceive(OnReceive, null);
    }

    void OnReceive(IAsyncResult result)
    {
        try
        {
            IPEndPoint remoteEP =
                new IPEndPoint(IPAddress.Any, port);

            byte[] data =
                udpClient.EndReceive(result, ref remoteEP);

            string message =
                Encoding.UTF8.GetString(data);

            if (message == "UNITY_HOST")
            {
                string hostIP =
                    remoteEP.Address.ToString();

                Debug.Log("Found Host: " + hostIP);

                OnHostFound?.Invoke(hostIP);
            }

            udpClient.BeginReceive(OnReceive, null);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void OnDestroy()
    {
        udpClient?.Close();
    }
}