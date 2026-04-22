using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMove : MonoBehaviour
{
    public float distance = 5f;
    public float speed = 3f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = Mathf.PingPong(Time.time * speed, distance);
        transform.position = new Vector3(startPos.x + x, transform.position.y, transform.position.z);
    }
}
