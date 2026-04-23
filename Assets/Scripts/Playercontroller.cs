using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
public enum PlayerState {Idle , Move , Jump , Falling}

public class Playercontroller : MonoBehaviour
{
    private float speed = 5f;
    private float jumForce = 5f;
    public Rigidbody2D _rd;
    private bool IsRight = false;
    public Transform model;
    public Animator amin;
    public bool IsGround;
    public LayerMask groundLayer;
    public Transform groundCheck;
    private int maxJump = 2;
    private int jumpCount = 0;

    private void Update()
    {
        Move();
        Jump();
    }

    private void Jump()
    {
        if(IsGround)
        {
            jumpCount = 0;   
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJump)
        {
            _rd.velocity = new Vector2(_rd.velocity.x, 0);
            _rd.AddForce(Vector2.up * jumForce, ForceMode2D.Impulse);
            jumpCount++;
        }
        amin.SetFloat("IsJump", Mathf.Abs(_rd.velocity.y));
        amin.SetBool("IsDoubleJump", jumpCount == maxJump);
    }
    private void Move()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) < 0.1f)
        {
            moveHorizontal = 0;
        }

        if (moveHorizontal < 0)
        {
            IsRight = false;
        }
        else if (moveHorizontal > 0)
        {
            IsRight = true;
        }

        if (IsRight)
        {
            model.localScale = new Vector3(1, 1, 1);
            _rd.velocity = new Vector3(moveHorizontal * speed, _rd.velocity.y);
        }
        else
        {
            model.localScale = new Vector3(-1, 1, 1);
            _rd.velocity = new Vector3(moveHorizontal * speed, _rd.velocity.y);
        }
        amin.SetFloat("IsRun", Mathf.Abs(moveHorizontal));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
                    EnemyMove.Instance.EnemyDie();
                    return;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGround = false;
        }
    }
}
