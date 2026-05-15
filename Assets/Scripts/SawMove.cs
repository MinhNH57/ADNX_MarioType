//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SawMove : MonoBehaviour
//{
//    public float distance = 5f;
//    public float speed = 3f;
//    private Vector3 startPos;

//    void Start()
//    {
//        startPos = transform.position;
//    }

//    void Update()
//    {
//        float x = Mathf.PingPong(Time.time * speed, distance);
//        transform.position = new Vector3(startPos.x + x, transform.position.y, transform.position.z);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SawMove : NetworkBehaviour
{
    public float distance = 5f;
    public float speed = 3f;
    private Vector3 startPos;

    private NetworkVariable<float> networkX = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    //public override void OnNetworkSpawn()
    //{
    //    startPos = transform.position;
    //    Debug.Log("Vị trí bắt đầu : " + transform.position);
    //}

    private void Start()
    {
        startPos = transform.position;
        Debug.Log("Vị trí bắt đầu : " + transform.position);
    }

    void Update()
    {
        if (IsServer) 
        {
            float x = Mathf.PingPong(Time.time * speed, distance);
            networkX.Value = x;
        }

        transform.position = new Vector3(
            startPos.x + networkX.Value,
            transform.position.y,
            transform.position.z
        );
    }
}