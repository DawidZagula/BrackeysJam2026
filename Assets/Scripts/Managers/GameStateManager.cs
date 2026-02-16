using System;

public class GameStateManager
{
    public GameState GetCurrentState => _currentState;
    private GameState _currentState;

    public event Action<GameState> OnStateChanged;

    public GameStateManager()
    {
        ChangeCurrentState(GameState.NotStarted);
    }

    public void ChangeCurrentState(GameState newState)
    {
        _currentState = newState;
        OnStateChanged?.Invoke(newState);
    }
}
