using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SawRotate : MonoBehaviour
{
    public AudioManager _audioManager;
    public float rotateSpeed = 200f;
    public GameObject _gameOverObject;
    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Update()
    {
        foreach(Transform child in transform)
        {
            child.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _audioManager.PlaySfx(_audioManager.failClip);
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
