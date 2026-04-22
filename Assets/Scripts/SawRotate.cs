using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SawRotate : MonoBehaviour
{
    public float rotateSpeed = 200f;
    public GameObject _gameOverObject;
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
            collision.gameObject.SetActive(false);      
            _gameOverObject.SetActive(true);
        }
    }
}
