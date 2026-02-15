using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("For debugging only")]
    [SerializeField] private GameState _currentState;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public GameState NewGameState { get; }

        public OnStateChangedEventArgs(GameState newState)
        {
            NewGameState = newState;
        }
    }

    private void Awake()
    {
        Instance = this;

        ChangeCurrentState(GameState.NotStarted);
    }

    public void ChangeCurrentState(GameState newState)
    {
        _currentState = newState;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(newState));
    }

    public GameState GetCurrentState() => _currentState;

}
