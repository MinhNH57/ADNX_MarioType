//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class SawRotate : MonoBehaviour
//{
//    public AudioManager _audioManager;
//    public float rotateSpeed = 200f;
//    public GameObject _gameOverObject;
//    private void Awake()
//    {
//        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
//    }
//    void Update()
//    {
//        foreach(Transform child in transform)
//        {
//            child.Rotate(0, 0, rotateSpeed * Time.deltaTime);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if(collision.CompareTag("Player"))
//        {
//            _audioManager.PlaySfx(_audioManager.failClip);
//            int finalScore = GameManager.Instance.coinCount;
//            Debug.Log("SawRotate" + finalScore);
//            GameManager.Instance.UpdateHighScore(finalScore);
//            StartCoroutine(LoadFail());
//            collision.gameObject.SetActive(false);
//            Debug.Log("SawRotate");
//            _gameOverObject.SetActive(true);
//        }
//    }

//    IEnumerator LoadFail()
//    {
//        yield return new WaitForSeconds(1f);
//    }
//}


using System.Collections;
using UnityEngine;

public class SawRotate : MonoBehaviour
{
    public AudioManager _audioManager;
    public float rotateSpeed = 200f;
    public GameObject _gameOverObject;

    private bool isTriggered = false; // ❗ tránh trigger nhiều lần

    private void Awake()
    {
        var audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null)
        {
            _audioManager = audioObj.GetComponent<AudioManager>();
        }
        else
        {
            Debug.LogError("❌ Không tìm thấy AudioManager");
        }
    }

    void Update()
    {
        foreach (Transform child in transform)
        {
            child.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered) return; // ❗ tránh gọi nhiều lần
        if (!collision.CompareTag("Player")) return;

        isTriggered = true;

        // 🔊 Play sound
        if (_audioManager != null)
        {
            _audioManager.PlaySfx(_audioManager.failClip);
        }

        int finalScore = GameManager.Instance.coinCount;
        Debug.Log("💀 Player chết | Score: " + finalScore);

        // ❗ Check player trước khi update
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHighScore(finalScore);
        }
        else
        {
            Debug.LogError("❌ GameManager NULL");
        }

        // ❗ disable player
        collision.gameObject.SetActive(false);

        // ❗ hiện game over UI
        if (_gameOverObject != null)
        {
            _gameOverObject.SetActive(true);
        }
        else
        {
            Debug.LogError("❌ GameOverObject chưa gán");
        }

        StartCoroutine(LoadFail());
    }

    IEnumerator LoadFail()
    {
        yield return new WaitForSeconds(1f);

        // 👉 Nếu muốn reload scene:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}