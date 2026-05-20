

//using Unity.Netcode;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameOver : MonoBehaviour
//{
//    public void LoadMainMenu()
//    {
//        if (NetworkManager.Singleton == null)
//        {
//            SceneManager.LoadScene("MainMenu");
//            return;
//        }

//        if (NetworkManager.Singleton.IsServer)
//        {
//            NetworkManager.Singleton.SceneManager.LoadScene(
//                "MainMenu",
//                LoadSceneMode.Single
//            );
//        }
//        else
//        {
//            RequestShutdownServerRpc();
//        }
//    }

//    [ServerRpc(RequireOwnership = false)]
//    private void RequestShutdownServerRpc()
//    {
//        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
//        {
//            var playerObj = client.PlayerObject;
//            if (playerObj != null)
//            {
//                playerObj.Despawn(true); 
//            }
//        }

//        NetworkManager.Singleton.SceneManager.LoadScene(
//            "MainMenu",
//            LoadSceneMode.Single
//        );
//    }
//}

using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameOver : NetworkBehaviour
{
    public void LoadMainMenu()
    {
        if (NetworkManager.Singleton == null)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        if (NetworkManager.Singleton.IsServer)
        {
            DespawnAllAndLoad();
        }
        else
        {
            RequestShutdownServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestShutdownServerRpc()
    {
        DespawnAllAndLoad();  
    }

    private void DespawnAllAndLoad()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject != null)
                client.PlayerObject.Despawn(true);
        }

        NetworkManager.Singleton.SceneManager.LoadScene(
            "MainMenu",
            LoadSceneMode.Single
        );
    }
}
