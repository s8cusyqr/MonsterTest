using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    

[Header("Movement")]
[SerializeField, Min(0f)] private float _moveSpeed = 3f;
[SerializeField, Min(0f)] private float _screenEdgeMargin = 30f;
[SerializeField, Min(0f)] private float _movementJitterStrength = 0.25f;

[Header("State")]
[SerializeField] private bool _startsEnabled = true;

private Rigidbody2D _rigidbody;
private Transform _playerTransform;
private Camera _camera;
private bool _isInitialized;
private Vector2 _jitterDirection;

public bool CanRun { get; private set; }

    public void Initialize(
        Transform playerTransform,
        Camera camera,
        float screenEdgeMargin)
    {
        _playerTransform = playerTransform;
        _camera = camera != null ? camera : Camera.main;
        _screenEdgeMargin = Mathf.Max(0f, screenEdgeMargin);
        _isInitialized = true;
        EnableMovement();
    }

    public void EnableMovement()
    {
        CanRun = true;
    }

    public void DisableMovement()
    {
        CanRun = false;
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        ConfigureRigidbody();
    }

    private void OnEnable()
    {
        CanRun = _startsEnabled;
        _jitterDirection = Random.insideUnitCircle.normalized * _movementJitterStrength;
    }

    private void FixedUpdate()
    {
        if (!CanRun || !_isInitialized || _playerTransform == null || _rigidbody == null)
        {
            return;
        }

        Vector2 desiredVelocity = CalculateDesiredVelocity();
        desiredVelocity = AdjustForScreenBounds(desiredVelocity);

        if (desiredVelocity.sqrMagnitude > Mathf.Epsilon)
        {
            _rigidbody.linearVelocity = desiredVelocity.normalized * _moveSpeed;
        }
        else
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }

        
    }

    private void ConfigureRigidbody()
    {
        if (_rigidbody == null)
        {
            Debug.LogError("Enemy requires a Rigidbody2D component.", this);
            return;
        }

        _rigidbody.gravityScale = 0f;
        _rigidbody.linearDamping = 0f;
        _rigidbody.angularDamping = 0f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.sleepMode = RigidbodySleepMode2D.StartAwake;
    }

    private Vector2 CalculateDesiredVelocity()
    {
        Vector2 awayFromPlayer = (Vector2)(transform.position - _playerTransform.position);
        if (awayFromPlayer.sqrMagnitude > Mathf.Epsilon)
        {
            awayFromPlayer.Normalize();
        }

        Vector2 desired = awayFromPlayer + _jitterDirection;
        return desired.sqrMagnitude > Mathf.Epsilon ? desired : awayFromPlayer;
    }

    private Vector2 AdjustForScreenBounds(Vector2 desiredVelocity)
    {
        if (_camera == null)
        {
            return desiredVelocity;
        }

        float halfHeight = _camera.orthographicSize;
        float halfWidth = halfHeight * _camera.aspect;
        float worldPerPixel = (2f * halfHeight) / Mathf.Max(1, Screen.height);
        float marginWorld = _screenEdgeMargin * worldPerPixel;

        Vector3 camPos = _camera.transform.position;
        float left = camPos.x - halfWidth + marginWorld;
        float right = camPos.x + halfWidth - marginWorld;
        float bottom = camPos.y - halfHeight + marginWorld;
        float top = camPos.y + halfHeight - marginWorld;

        Vector3 pos = transform.position;

        if (pos.x <= left && desiredVelocity.x < 0f)
        {
            desiredVelocity.x = 0f;
        }

        if (pos.x >= right && desiredVelocity.x > 0f)
        {
            desiredVelocity.x = 0f;
        }

        if (pos.y <= bottom && desiredVelocity.y < 0f)
        {
            desiredVelocity.y = 0f;
        }

        if (pos.y >= top && desiredVelocity.y > 0f)
        {
            desiredVelocity.y = 0f;
        }

        return desiredVelocity;
    }

   
}
