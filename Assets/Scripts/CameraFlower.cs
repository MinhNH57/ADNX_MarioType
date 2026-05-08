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
    private bool isInitialized = false;

    void Start()
    {
        // Không khởi tạo offset ở đây nữa vì ban đầu chưa có target
        float camHeight = Camera.main.orthographicSize * 2;
        float camWidth = camHeight * Camera.main.aspect;
        camHalfWidth = camWidth / 2;
    }

    // Hàm này sẽ được gọi từ script Player khi Player đó là Local Player
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        offset = transform.position - target.position;
        lowY = transform.position.y;
        isInitialized = true;
    }

    void LateUpdate()
    {
        if (target == null || !isInitialized) return;

        Vector3 targetPos = target.position + offset;

        // Giới hạn biên X nếu có landmark
        if (landmarkObject != null)
        {
            float maxX = landmarkObject.position.x - camHalfWidth;
            targetPos.x = Mathf.Min(targetPos.x, maxX);
        }

        // Di chuyển mượt mà
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothing * Time.deltaTime
        );

        // Giới hạn trục Y (không cho camera xuống quá thấp)
        if (smoothedPosition.y < lowY)
        {
            smoothedPosition.y = lowY;
        }

        transform.position = smoothedPosition;
    }
}