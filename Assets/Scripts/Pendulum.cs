using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject _gameOverObject;
    public float speed = 2f;     // tốc độ đung
    public float angle = 45f;    // góc tối đa

    void Update()
    {
        float z = Mathf.Sin(Time.time * speed) * angle;
        transform.rotation = Quaternion.Euler(0, 0, z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            if(_gameOverObject != null)
            {
                _gameOverObject.SetActive(true);
            }
            else
            {
                Debug.Log("_gameOverObject is null");
            }
        }
    }
}
