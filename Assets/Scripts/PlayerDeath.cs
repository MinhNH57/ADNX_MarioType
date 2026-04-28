using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public AudioManager _audioManager;
    public GameObject _gameOverObject;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _audioManager.PlaySfx(_audioManager.failClip);

            int finalScore = GameManager.Instance.coinCount;
            Debug.Log(finalScore);
            GameManager.Instance.UpdateHighScore(finalScore);

            StartCoroutine(LoadFail());

            collision.gameObject.SetActive(false);

            _gameOverObject.SetActive(true);
        }
    }

    IEnumerator LoadFail()
    {
        yield return new WaitForSeconds(1f);
    }
}
