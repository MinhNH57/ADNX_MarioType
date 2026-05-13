using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject _gameOverObject;
    private void Start()
    {
        if (_gameOverObject != null)
        {
            Debug.Log("Đã chạy vào đây để tắt panel Over lần đầu");
            _gameOverObject.SetActive(false);
        }
    }
}