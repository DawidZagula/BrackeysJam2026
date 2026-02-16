using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundMover : MonoBehaviour
{

    [Header("Collider Customization")]
    [SerializeField] private float _deathAreaHeight;

    [Header("Behaviour Customization")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _speedIncreasePercent;

    //Run-time state
    [Header("For debugging only")]
    [SerializeField] private float _currentMoveSpeed;
    private bool _isMoving;

    //Components
    private BoxCollider2D _deathAreaCollider;

    private void Awake()
    {
        _deathAreaCollider = GetComponent<BoxCollider2D>();

        _currentMoveSpeed = _minSpeed;
    }

    private void Start()
    {
        SubscribeEvents();

        SetDeathAreaSize();
    }

    private void SubscribeEvents()
    {
        GameStateManager.Instance.OnStateChanged
            += GameStateManager_OnStateChanged;

        DimensionStateHolder.Instance.OnDimensionChanged
            += DimensionStateHolder_OnDimensionChanged;

        PickupHolder.Instance.OnUsedPickup 
            += PickupHolder_OnUsedPickup;
    }

    private void GameStateManager_OnStateChanged(object sender, GameStateManager.OnStateChangedEventArgs e)
    {
        if (e.NewGameState == GameState.Started)
        {
            _isMoving = true;
            return;
        }
        _isMoving = false;
    }
    private void DimensionStateHolder_OnDimensionChanged(object sender, DimensionStateHolder.OnDimensionChangedEventArgs e)
    {
        IncreaseSpeed();
    }

    private void PickupHolder_OnUsedPickup(object sender, EventArgs e)
    {
        DecreaseSpeed();
    }

    private void SetDeathAreaSize()
    {
        float screenWidth = (Camera.main.orthographicSize * 2) * Camera.main.aspect;

        _deathAreaCollider.size = new Vector2(screenWidth, _deathAreaHeight);
    }

    private void Update()
    {
        if (!_isMoving) { return; }

        MoveUp();

        //For debugging
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            IncreaseSpeed();
        }
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            DecreaseSpeed();
        }
    }

    private void MoveUp()
    {
        float deltaY = _currentMoveSpeed * Time.deltaTime;
        float positionX = Camera.main.transform.position.x;

        Vector3 deltaPosition = new Vector3(positionX, transform.position.y + deltaY, 0);  

        transform.position = deltaPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerDeathZoneRegister player))
        {
            player.ProceedDeath();
        }
    }

    private void IncreaseSpeed()
    {
        float multiplier = 1f + _speedIncreasePercent;
        _currentMoveSpeed *= multiplier;

        _currentMoveSpeed = Mathf.Min(_currentMoveSpeed, _maxSpeed);
    }

    private void DecreaseSpeed()
    {
        float multiplier = 1f + _speedIncreasePercent;
        _currentMoveSpeed /= multiplier;

        _currentMoveSpeed = Mathf.Max(_currentMoveSpeed, _minSpeed);
    }

    public float GetDeathAreaHeight() => _deathAreaHeight;

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void UnsubscribeEvents()
    {
        GameStateManager.Instance.OnStateChanged 
            -= GameStateManager_OnStateChanged;

        DimensionStateHolder.Instance.OnDimensionChanged
          -= DimensionStateHolder_OnDimensionChanged;

        PickupHolder.Instance.OnUsedPickup
          -= PickupHolder_OnUsedPickup;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawCube(transform.position,
            new Vector2((Camera.main.orthographicSize * 2) * Camera.main.aspect,
            _deathAreaHeight));
    }
}
