using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string _nextScene;
    private bool isLoading = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLoading) return;

        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(_nextScene);
        }
    }
}
