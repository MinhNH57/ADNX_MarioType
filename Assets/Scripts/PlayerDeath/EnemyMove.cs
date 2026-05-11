//using UnityEngine;

//public class EnemyMove : MonoBehaviour
//{
//    //public static EnemyMove Instance;
//    public float speed = 2f;
//    public float distance = 4f;

//    private float startX;
//    private int direction = 1;

//    //void Awake()
//    //{
//    //    Instance = this;
//    //}
//    void Start()
//    {
//        startX = transform.position.x;
//    }

//    void Update()
//    {
//        float newX = transform.position.x + direction * speed * Time.deltaTime;

//        float minX = startX - distance;
//        float maxX = startX + distance;


//        if (newX > maxX)
//        {
//            newX = maxX;
//            direction = -1;
//        }
//        else if (newX < minX)
//        {
//            newX = minX;
//            direction = 1;
//        }
//        transform.localScale = new Vector3(direction, 1, 1);
//        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
//    }

//    public void EnemyDie()
//    {
//        GetComponent<Collider2D>().enabled = false;
//        transform.localScale = new Vector3(1, 0.3f, 1);
//        GameManager.Instance.AddCoin(3);
//        Destroy(gameObject, 0.2f);
//    }
//}

using Unity.Netcode;
using UnityEngine;

public class EnemyMove : NetworkBehaviour
{
    public float speed = 2f;
    public float distance = 4f;

    private float startX;
    private int direction = 1;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        if (!IsServer) return;

        float newX =
            transform.position.x +
            direction * speed * Time.deltaTime;

        float minX = startX - distance;
        float maxX = startX + distance;

        if (newX > maxX)
        {
            newX = maxX;
            direction = -1;
        }
        else if (newX < minX)
        {
            newX = minX;
            direction = 1;
        }

        transform.localScale =
            new Vector3(direction, 1, 1);

        transform.position =
            new Vector3(
                newX,
                transform.position.y,
                transform.position.z
            );
    }

    [ServerRpc(RequireOwnership = false)]
    public void KillEnemyServerRpc()
    {
        EnemyDie();
    }

     public void EnemyDie()
    {
        GetComponent<Collider2D>().enabled = false;

        transform.localScale =
            new Vector3(1, 0.3f, 1);

        GameManager.Instance.AddCoin(3);

        GetComponent<NetworkObject>().Despawn();
    }
}