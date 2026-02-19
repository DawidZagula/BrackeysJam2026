using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputReader : IDisposable
{
    private PlayerInputActions _playerInputActions;

    private bool _allowInput;

    public event EventHandler<MoveEventArgs> OnMove;
    public class MoveEventArgs : EventArgs { public float MoveInput { get; set; } }

    public event EventHandler OnJumpPressed;
    public event EventHandler OnJumpReleased;

    public event EventHandler OnDimensionChangePressed;

    public event EventHandler OnTryUsePickup;

    private readonly GameStateManager _gameStateManager;

    public InputReader(GameStateManager gameStateManager)
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _gameStateManager = gameStateManager;
        _gameStateManager.OnStateChanged += OnStateChanged;
        SubscribeGameplayInputEvents();
    }



    private void OnStateChanged(GameState gameState)
    {
        if (gameState == GameState.Started)
        {
            _allowInput = true;
            return;
        }
        else if (gameState == GameState.GameOver 
            || gameState == GameState.NotStarted 
            || gameState == GameState.Cutscene
            || gameState == GameState.Paused)
        {
            _allowInput = false;
        }
    }

    private void SubscribeGameplayInputEvents()
    {
        _playerInputActions.Player.Move.started += Move_started;
        _playerInputActions.Player.Move.canceled += Move_canceled;

        _playerInputActions.Player.Jump.started += Jump_started;
        _playerInputActions.Player.Jump.canceled += Jump_canceled;

        _playerInputActions.Player.ChangeDimension.started += ChangeDimension_started;

        _playerInputActions.Player.TryUsePickup.started += TryUsePickup_started;
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        if (!_allowInput)
        {
            OnMove?.Invoke(this, new MoveEventArgs { MoveInput = 0f });
            return;
        }

        float moveInput = obj.ReadValue<float>();
        OnMove?.Invoke(this, new MoveEventArgs { MoveInput = moveInput });
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        OnMove?.Invoke(this, new MoveEventArgs { MoveInput = 0f });
    }
    private void Jump_started(InputAction.CallbackContext obj)
    {
        if (!_allowInput)
        {
            OnJumpReleased?.Invoke(this, EventArgs.Empty);
            return;
        }
        OnJumpPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_canceled(InputAction.CallbackContext obj)
    {
        OnJumpReleased?.Invoke(this, EventArgs.Empty);
    }

    private void ChangeDimension_started(InputAction.CallbackContext obj)
    {
       if (!_allowInput) { return; }
        
        OnDimensionChangePressed?.Invoke(this, EventArgs.Empty);
    }

    private void TryUsePickup_started(InputAction.CallbackContext obj)
    {
        if (!_allowInput) { return; }

        OnTryUsePickup?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        UnsubscribeEvents();
    }

    private void UnsubscribeEvents()
    {
        _playerInputActions.Player.Move.started -= Move_started;
        _playerInputActions.Player.Move.canceled -= Move_canceled;

        _playerInputActions.Player.Jump.started -= Jump_started;
        _playerInputActions.Player.Jump.canceled -= Jump_canceled;

        _playerInputActions.Player.ChangeDimension.started -= ChangeDimension_started;

        _playerInputActions.Player.TryUsePickup.started -= TryUsePickup_started;

        _gameStateManager.OnStateChanged -= OnStateChanged;

        _playerInputActions.Player.Disable();
    }
}
