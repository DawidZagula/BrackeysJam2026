using System.Collections;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    [Header("Environment Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Wall Check")]
    [SerializeField] private float _wallCheckDistance = 0.1f;

    private enum JumpOrigin
    {
        None,
        Player,
        Trampoline
    }

    private JumpOrigin _currentJumpOrigin = JumpOrigin.None;

    // Components
    [SerializeField] private PlayerMovementData _configurationData;
    private Rigidbody2D _rigidbody;
    private KnockbackReceiver _knockbackReceiver;

    // State Parameters
    private bool _isFacingRight;
    private bool _isJumping;
    private bool _isJumpCut;
    private bool _isJumpFalling;

    // Timers
    private float _lastOnGroundTime;
    private float _lastPressedJumpTime;

    // Input Parameters
    private Vector2 _moveInput;

    private void Awake()
    {
        SetInitialState();
    }

    private void SetInitialState()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _knockbackReceiver = GetComponent<KnockbackReceiver>();
        SetGravityScale(_configurationData.gravityScale);
        _isFacingRight = true;

        // Preveent wall sticking
        PhysicsMaterial2D noFriction = new PhysicsMaterial2D("PlayerNoFriction");
        noFriction.friction = 0f;
        noFriction.bounciness = 0f;
        _rigidbody.sharedMaterial = noFriction;

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null && boxCollider.edgeRadius == 0f)
        {
            boxCollider.edgeRadius = 0.02f;
            boxCollider.size -= Vector2.one * 0.02f * 2f;
        }
    }

    private void Start()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        InputReader.Instance.OnMove += InputReader_OnMove;
        InputReader.Instance.OnJumpPressed += InputReader_OnJumpPressed;
        InputReader.Instance.OnJumpReleased += InputReader_OnJumpReleased;
    }

    private void InputReader_OnMove(object sender, InputReader.MoveEventArgs e)
    {
        _moveInput.x = e.MoveInput;
    }

    private void InputReader_OnJumpPressed(object sender, System.EventArgs e)
    {
        _lastPressedJumpTime = _configurationData.jumpInputBufferTime;
    }

    private void InputReader_OnJumpReleased(object sender, System.EventArgs e)
    {
        if (_knockbackReceiver.IsBeingKnockedBack) { return; }

        if (CanJumpCut())
            _isJumpCut = true;
    }

    private void Update()
    {

        _lastOnGroundTime -= Time.deltaTime;
        _lastPressedJumpTime -= Time.deltaTime;

        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
        {
            if (!_isJumping)
            {
                _lastOnGroundTime = _configurationData.coyoteTime;
            }
        }

        if (_isJumping && _rigidbody.linearVelocity.y < 0)
        {
            SetFallingState();
        }

        if (_lastOnGroundTime > 0 && !_isJumping)
        {
            ResetJumpState();
        }

        UpdateGravity();

    }

    private void SetFallingState()
    {
        _isJumping = false;
        _isJumpFalling = true;
    }

    private void ResetJumpState()
    {
        _currentJumpOrigin = JumpOrigin.None;

        _isJumpCut = false;
        _isJumpFalling = false;
    }

    private void UpdateGravity()
    {
        if (_isJumpCut)
        {
            SetGravityScale(_configurationData.gravityScale * _configurationData.jumpCutGravityMultiplier);
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, Mathf.Max(_rigidbody.linearVelocity.y, -_configurationData.maxFallSpeed));
        }
        else if (_isJumping || _isJumpFalling)
        {
            SetGravityScale(_configurationData.gravityScale);
        }
        else if (_rigidbody.linearVelocity.y < 0)
        {
            SetGravityScale(_configurationData.gravityScale * _configurationData.fallGravityMultiplier);
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, Mathf.Max(_rigidbody.linearVelocity.y, -_configurationData.maxFallSpeed));
        }
        else
        {
            SetGravityScale(_configurationData.gravityScale);
        }
    }

    private void SetGravityScale(float scale)
    {
        _rigidbody.gravityScale = scale;
    }

    private void FixedUpdate()
    {
        if (_knockbackReceiver.IsBeingKnockedBack) { return; }

        if (_moveInput.x != 0)
        {
            UpdateFaceDirection();
        }

        UpdateMovement();

        if (CanJump() && _lastPressedJumpTime > 0)
        {
            _isJumping = true;
            PerformJump();
        }
    }

    private void UpdateFaceDirection()
    {

        if (_moveInput.x > 0 && !_isFacingRight
            ||
            _moveInput.x < 0 && _isFacingRight)
        {
            Turn();
        }
    }
    private void Turn()
    {
        transform.Rotate(0, 180, 0);
        _isFacingRight = !_isFacingRight;
    }

    private void UpdateMovement()
    {
        const float acceleration = 50f;

        float targetSpeed = _moveInput.x * _configurationData.moveSpeed;

        // Prevents wall sticking while airborne
        if (_moveInput.x != 0 && IsPushingIntoWall())
        {
            targetSpeed = 0f;
        }

        float speedDifference = targetSpeed - _rigidbody.linearVelocity.x;

        _rigidbody.AddForce(Vector2.right * speedDifference * acceleration, ForceMode2D.Force);
    }

    private bool IsPushingIntoWall()
    {
        Collider2D col = GetComponent<Collider2D>();
        float direction = _moveInput.x > 0 ? 1f : -1f;
        RaycastHit2D hit = Physics2D.Raycast(
            col.bounds.center,
            Vector2.right * direction,
            col.bounds.extents.x + _wallCheckDistance,
            _groundLayer
        );
        return hit.collider != null;
    }

    private bool CanJump()
    {
        return _lastOnGroundTime > 0 && !_isJumping;
    }

    private void PerformJump()
    {
        _currentJumpOrigin = JumpOrigin.Player;

        ResetJumpTimers();

        float jumpForce = _configurationData.jumpForce;
        if (_rigidbody.linearVelocity.y < 0)
            jumpForce -= _rigidbody.linearVelocity.y;

        _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ResetJumpTimers()
    {
        _lastPressedJumpTime = 0;
        _lastOnGroundTime = 0;
    }

    private bool CanJumpCut()
    {
        return
            _isJumping
            && _rigidbody.linearVelocity.y > 0
            && _currentJumpOrigin == JumpOrigin.Player
            && _lastOnGroundTime <= 0;
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void LaunchFromTrampoline(float trampolineVelocity)
    {
        _currentJumpOrigin = JumpOrigin.Trampoline;

        _rigidbody.linearVelocity =
            new Vector2(_rigidbody.linearVelocity.x, trampolineVelocity);

        _isJumping = true;
        _isJumpCut = false;
        _isJumpFalling = false;

        ResetJumpTimers();
    }

    private void UnsubscribeEvents()
    {
        InputReader.Instance.OnMove -= InputReader_OnMove;
        InputReader.Instance.OnJumpPressed -= InputReader_OnJumpPressed;
        InputReader.Instance.OnJumpReleased -= InputReader_OnJumpReleased;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    }
}
