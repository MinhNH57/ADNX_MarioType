using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Pendulum : NetworkBehaviour
{
    [Header("UI")]
    public GameObject gameOverObject;

    [Header("Animation")]
    public float speed = 2f;
    public float angle = 45f;

    [Header("Audio")]
    public AudioManager audioManager;

    private bool canKill = false;

    private void Awake()
    {
        if (audioManager == null)
        {
            GameObject audioObj =
                GameObject.FindGameObjectWithTag("Audio");

            if (audioObj != null)
            {
                audioManager =
                    audioObj.GetComponent<AudioManager>();
            }
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        canKill = true;
    }

    private void Update()
    {
        // CHỈ SERVER điều khiển pendulum
        if (!IsServer) return;

        float z =
            Mathf.Sin(Time.time * speed)
            * angle;

        transform.rotation =
            Quaternion.Euler(0, 0, z);
    }

    private void OnTriggerEnter2D(
        Collider2D collision)
    {
        // CHỈ SERVER xử lý va chạm
        if (!IsServer) return;

        if (!canKill) return;

        if (!collision.CompareTag("Player"))
            return;

        NetworkObject playerNetObj =
            collision.GetComponent<NetworkObject>();

        if (playerNetObj == null)
            return;

        if (!playerNetObj.IsSpawned)
            return;

        HandleDeath(playerNetObj);
    }

    private void HandleDeath(
        NetworkObject playerNetObj)
    {
        ulong deadClientId =
            playerNetObj.OwnerClientId;

        Debug.Log(
            $"PLAYER DEAD: {deadClientId}"
        );

        // Chỉ gửi GameOver cho client chết
        ClientRpcParams rpcParams =
            new ClientRpcParams
            {
                Send =
                    new ClientRpcSendParams
                    {
                        TargetClientIds =
                            new ulong[]
                            {
                                deadClientId
                            }
                    }
            };

        ShowGameOverClientRpc(rpcParams);

        // SERVER despawn player
        if (playerNetObj != null &&
            playerNetObj.IsSpawned)
        {
            playerNetObj.Despawn(true);
        }
    }

    [ClientRpc]
    private void ShowGameOverClientRpc(
        ClientRpcParams rpcParams = default)
    {
        // Chỉ owner local mới hiện UI
        if (!IsOwner) return;

        Debug.Log("GAME OVER");

        if (audioManager != null)
        {
            audioManager.PlaySfx(
                audioManager.failClip
            );
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHighScore(
                GameManager.Instance.coinCount
            );
        }

        if (gameOverObject != null)
        {
            gameOverObject.SetActive(true);
        }

        StartCoroutine(LoadFail());
    }

    private IEnumerator LoadFail()
    {
        yield return new WaitForSeconds(1f);
    }
}