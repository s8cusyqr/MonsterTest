using UnityEngine;

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)] private float _movementSpeed = 5f;
    [SerializeField, Min(0f)] private float _screenEdgeMargin = 1f;

    private Camera _gameCamera;

    public void Initialize(Camera camera)
    {
        _gameCamera = camera;
    }

    private void Awake()
    {
        if (_gameCamera == null)
        {
            _gameCamera = Camera.main;
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_gameCamera == null)
        {
            return;
        }

        float step = _movementSpeed * Time.deltaTime;
        Vector3 screenPos = _gameCamera.WorldToScreenPoint(transform.position);

        if (Input.GetKey(KeyCode.W) && screenPos.y < Screen.height - _screenEdgeMargin)
        {
            transform.position += Vector3.up * step;
        }

        if (Input.GetKey(KeyCode.S) && screenPos.y > _screenEdgeMargin)
        {
            transform.position += Vector3.down * step;
        }

        if (Input.GetKey(KeyCode.D) && screenPos.x < Screen.width - _screenEdgeMargin)
        {
            transform.position += Vector3.right * step;
        }

        if (Input.GetKey(KeyCode.A) && screenPos.x > _screenEdgeMargin)
        {
            transform.position += Vector3.left * step;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DisableMovement();
            }
        }
    }
}
