using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

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
    private DimensionStateHolder _dimensionStateHolder;
    private LavaFreezer _lavaFreezer;
    
    private float _freezeRemainingTime = 0f;
    private bool _isFrozen = false;
    
    [Inject]
    public void Construct( 
        DimensionStateHolder dimensionStateHolder,
        LavaFreezer lavaFreezer)
    {
        _dimensionStateHolder = dimensionStateHolder;
        _lavaFreezer = lavaFreezer;
    }
    private void FreezeLava(float freezeDuration)
    {
        _freezeRemainingTime += freezeDuration;
        if (!_isFrozen)
        {
            _isFrozen = true;
            _isMoving = false;
        }
    }
    
    private void Awake()
    {
        _deathAreaCollider = GetComponent<BoxCollider2D>();

        _currentMoveSpeed = _minSpeed;
        _isMoving = true;
    }

    private void Start()
    {
        SubscribeEvents();

        SetDeathAreaSize();
    }

    private void SubscribeEvents()
    {
        _dimensionStateHolder.OnDimensionChanged
            += DimensionStateHolder_OnDimensionChanged;

        PickupHolder.Instance.OnUsedPickup 
            += PickupHolder_OnUsedPickup;
        
        _lavaFreezer.OnLavaFrozen += FreezeLava;
    }

    private void DimensionStateHolder_OnDimensionChanged(Dimension dimension)
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
        HandleFreezeTimer();
        
        if (!_isMoving) { return; }

        MoveUp();
    }

    private void HandleFreezeTimer()
    {
        if (!_isFrozen) return;

        _freezeRemainingTime -= Time.deltaTime;

        if (_freezeRemainingTime < 0f)
        {
            _freezeRemainingTime = 0f;
            _isFrozen = false;
            _isMoving = true;
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
        if (collision.TryGetComponent(out PlayerHealth player))
        {
            player.ProcessDeath();
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
        _dimensionStateHolder.OnDimensionChanged
          -= DimensionStateHolder_OnDimensionChanged;

        PickupHolder.Instance.OnUsedPickup
          -= PickupHolder_OnUsedPickup;
        
        _lavaFreezer.OnLavaFrozen -= FreezeLava;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(transform.position,
            new Vector2((Camera.main.orthographicSize * 2) * Camera.main.aspect,
            _deathAreaHeight));
    }
}
