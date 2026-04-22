
using UnityEngine;

public class CameraFlower : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float smoothing = 5f;

    [Header("Limit")]
    public float lowY;

    [Header("Lock Camera")]
    public Transform landmarkObject; 

    private Vector3 offset;
    private float camHalfWidth;

    void Start()
    {
        offset = transform.position - target.position;
        lowY = transform.position.y;
        float camHeight = Camera.main.orthographicSize * 2;
        float camWidth = camHeight * Camera.main.aspect;
        camHalfWidth = camWidth / 2;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;

        float maxX = landmarkObject.position.x - camHalfWidth;
        targetPos.x = Mathf.Min(targetPos.x, maxX);
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothing * Time.deltaTime
        );
        if (transform.position.y < lowY)
        {
            transform.position = new Vector3(
                transform.position.x,
                lowY,
                transform.position.z
            );
        }
    }
}