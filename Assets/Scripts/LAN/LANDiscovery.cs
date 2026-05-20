

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

    private bool isRunning = false;

    public Action<string> OnHostFound;

    private void Awake()
    {
        Instance = this;
    }

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

    public void StartListening()
    {
        udpClient = new UdpClient(port);

        isRunning = true;

        udpClient.BeginReceive(OnReceive, null);
    }

    void OnReceive(IAsyncResult result)
    {
        try
        {
            if (!isRunning || udpClient == null)
                return;

            IPEndPoint remoteEP =
                new IPEndPoint(IPAddress.Any, port);

            byte[] data =
                udpClient.EndReceive(result, ref remoteEP);

            string message =
                Encoding.UTF8.GetString(data);

            if (message == "UNITY_HOST")
            {

                string hostIP = remoteEP.Address.ToString();
                if(IsLocalIP(hostIP))
                {
                    Debug.Log("Đang nhận dữ liệu từ chính mình");
                }
                else
                {
                    Debug.Log("Found Host: " + hostIP);

                    OnHostFound?.Invoke(hostIP);
                }
            }

            if (isRunning && udpClient != null)
            {
                udpClient.BeginReceive(OnReceive, null);
            }
        }
        catch (ObjectDisposedException)
        {
            Debug.Log("UDP socket đã đóng.");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void OnDestroy()
    {
        isRunning = false;

        udpClient?.Close();

        udpClient = null;
    }

    private bool IsLocalIP(string ip)
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress localIP in host.AddressList)
        {
            if (localIP.AddressFamily == AddressFamily.InterNetwork)
            {
                if (localIP.ToString() == ip)
                    return true;
            }
        }
        return false;
    }
}