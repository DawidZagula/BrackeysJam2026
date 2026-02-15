using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public static InputReader Instance { get; private set; }

    private PlayerInputActions _playerInputActions;


    public event EventHandler<MoveEventArgs> OnMove;
    public class MoveEventArgs : EventArgs { public float MoveInput { get; set; } }

    public event EventHandler OnJumpPressed;
    public event EventHandler OnJumpReleased;


    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

    }

    private void Start()
    {
        GameStateManager.Instance.OnStateChanged += GameStateManager_OnStateChanged;
    }

    private void GameStateManager_OnStateChanged(object sender, GameStateManager.OnStateChangedEventArgs e)
    {
        if (e.NewGameState == GameState.Started)
        {
            _playerInputActions.Player.Move.started += Move_started;
            _playerInputActions.Player.Move.canceled += Move_canceled;

            _playerInputActions.Player.Jump.started += Jump_started;
            _playerInputActions.Player.Jump.canceled += Jump_canceled;
            return;
        }
        else if (e.NewGameState == GameState.GameOver)
        {
            OnMove?.Invoke(this, new MoveEventArgs { MoveInput = 0f });
            OnJumpReleased?.Invoke(this, EventArgs.Empty);

            _playerInputActions.Player.Move.started -= Move_started;
            _playerInputActions.Player.Move.canceled -= Move_canceled;

            _playerInputActions.Player.Jump.started -= Jump_started;
            _playerInputActions.Player.Jump.canceled -= Jump_canceled;

            _playerInputActions.Player.Disable();
        }
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        float moveInput = obj.ReadValue<float>();
        OnMove?.Invoke(this, new MoveEventArgs { MoveInput = moveInput });
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        OnMove?.Invoke(this, new MoveEventArgs { MoveInput = 0f });
    }
    private void Jump_started(InputAction.CallbackContext obj)
    {
        OnJumpPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_canceled(InputAction.CallbackContext obj)
    {
        OnJumpReleased?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Move.started -= Move_started;
        _playerInputActions.Player.Move.canceled -= Move_canceled;


        _playerInputActions.Player.Jump.started -= Jump_started;
        _playerInputActions.Player.Jump.canceled -= Jump_canceled;

        _playerInputActions.Player.Disable();
    }

}
