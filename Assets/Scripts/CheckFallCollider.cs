using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckFallCollider : MonoBehaviour
{
    public GameObject _gameOver;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D point in collision.contacts)
        {
            if (Mathf.Abs(point.point.y - (-5.12f)) < 0.05f)
            { 
                collision.gameObject.SetActive(false);
                _gameOver.SetActive(true);
                return;
            }
        }
    }
}
