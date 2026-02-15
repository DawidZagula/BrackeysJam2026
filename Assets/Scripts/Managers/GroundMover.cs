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

    //Components
    private BoxCollider2D _deathAreaCollider;

    private void Awake()
    {
        _deathAreaCollider = GetComponent<BoxCollider2D>();

        _currentMoveSpeed = _minSpeed;
    }

    private void Start()
    {
        SetDeathAreaSize();
    }

    private void SetDeathAreaSize()
    {
        float screenWidth = (Camera.main.orthographicSize * 2) * Camera.main.aspect;

        _deathAreaCollider.size = new Vector2(screenWidth, _deathAreaHeight);
    }

    private void Update()
    {
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
        Vector2 moveDelta = Vector2.up * _currentMoveSpeed * Time.deltaTime;

        transform.Translate(moveDelta);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMover player))
        {
            Debug.Log("Player hit");
        }
    }

    public void IncreaseSpeed()
    {
        float multiplier = 1f + _speedIncreasePercent;
        _currentMoveSpeed *= multiplier;

        _currentMoveSpeed = Mathf.Min(_currentMoveSpeed, _maxSpeed);
    }

    public void DecreaseSpeed()
    {
        float multiplier = 1f + _speedIncreasePercent;
        _currentMoveSpeed /= multiplier;

        _currentMoveSpeed = Mathf.Max(_currentMoveSpeed, _minSpeed);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, 
            new Vector2((Camera.main.orthographicSize * 2) * Camera.main.aspect, 
            _deathAreaHeight));
    }
}
