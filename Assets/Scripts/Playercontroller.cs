using Unity.Netcode; // THÊM THƯ VIỆN NÀY
using UnityEngine;
using System.Collections;

public class Playercontroller : NetworkBehaviour // ĐỔI TỪ MonoBehaviour SANG NetworkBehaviour
{
    public AudioManager _audioManager;
    public GameObject _gameOverObject;
    private float speed = 5f;
    private float jumForce = 5f;
    public Rigidbody2D _rd;
    private bool IsRight = false;
    public Transform model;
    public Animator amin;
    public bool IsGround;
    private int maxJump = 2;
    private int jumpCount = 0;
    public float offsetDeath = -6f;
    private Camera cam;

    private void Awake()
    {
        var audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null) _audioManager = audioObj.GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        Move();
        Jump();
        CheckFallDeath();
    }

    private void Jump()
    {
        if (IsGround) jumpCount = 0;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJump)
        {
            _rd.velocity = new Vector2(_rd.velocity.x, 0);
            _rd.AddForce(Vector2.up * jumForce, ForceMode2D.Impulse);
            jumpCount++;

            if (jumpCount == 2)
            {
                amin.Play("DoubleJump");
            }
        }
        amin.SetFloat("IsJump", Mathf.Abs(_rd.velocity.y));
        amin.SetBool("IsDoubleJump", jumpCount == maxJump);
    }

    private void Move()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) < 0.1f) moveHorizontal = 0;

        if (moveHorizontal < 0) IsRight = false;
        else if (moveHorizontal > 0) IsRight = true;

        model.localScale = IsRight ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

        _rd.velocity = new Vector2(moveHorizontal * speed, _rd.velocity.y);

        amin.SetFloat("IsRun", Mathf.Abs(moveHorizontal));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsOwner) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGround = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y == 1f)
                {
                    EnemyMove enemy = collision.gameObject.GetComponent<EnemyMove>();
                    if (enemy != null)
                    {
                        enemy.KillEnemyServerRpc();
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsOwner) return;
        if (collision.gameObject.CompareTag("Ground")) IsGround = false;
    }

    private void CheckFallDeath()
    {
        if (!IsOwner) return;
        float deathZoneY = -10f;

        if (transform.position.y < deathZoneY)
        {
            RequestGameOverServerRpc();
        }
    }

    [ServerRpc]
    private void RequestGameOverServerRpc()
    {
        NotifyAllPlayersDeathClientRpc();
    }

    [ClientRpc]
    private void NotifyAllPlayersDeathClientRpc()
    {
        Debug.Log("Một thành viên hy sinh, cả đội Game Over!");

        if (_audioManager != null)
            _audioManager.PlaySfx(_audioManager.failClip);

        if (_gameOverObject != null)
            _gameOverObject.SetActive(true);

        this.enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            CameraFlower camScript = Camera.main.GetComponent<CameraFlower>();
            if (camScript != null)
            {
                camScript.SetTarget(this.transform);
            }
        }
    }
}