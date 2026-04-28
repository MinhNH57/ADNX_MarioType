using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject _gameOverObject;
    public float speed = 2f; 
    public float angle = 45f;   
    private bool isTriggered = false;
    public AudioManager _audioManager;

    void Update()
    {
        float z = Mathf.Sin(Time.time * speed) * angle;
        transform.rotation = Quaternion.Euler(0, 0, z);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        collision.gameObject.SetActive(false);
    //        if(_gameOverObject != null)
    //        {
    //            _gameOverObject.SetActive(true);
    //        }
    //        else
    //        {
    //            Debug.Log("_gameOverObject is null");
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered) return;
        if (!collision.CompareTag("Player")) return;

        isTriggered = true;

        if (_audioManager != null)
        {
            _audioManager.PlaySfx(_audioManager.failClip);
        }

        int finalScore = GameManager.Instance.coinCount;
        Debug.Log("💀 Player chết | Score: " + finalScore);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHighScore(finalScore);
        }
        else
        {
            Debug.LogError("❌ GameManager NULL");
        }

        collision.gameObject.SetActive(false);

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
    }
}
