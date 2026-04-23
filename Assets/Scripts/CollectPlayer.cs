using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPlayer : MonoBehaviour
{
    public AudioManager _audioManager;
    public int value = 1;
    public float moveUpSpeed = 2f;
    public float lifeTime = 0.5f;
    private bool isCollected = false;
    private float timer = 0f;
    private SpriteRenderer sr;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isCollected) return;

        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / lifeTime);
        sr.color = new Color(1, 1, 1, alpha);

        if (timer >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (GameManager.Instance != null)
            _audioManager.PlaySfx(_audioManager.coinClip);
            GameManager.Instance.AddCoin(value);

        isCollected = true;

        GetComponent<Collider2D>().enabled = false;
    }
}
