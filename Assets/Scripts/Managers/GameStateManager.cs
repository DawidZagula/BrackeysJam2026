using System;

public class GameStateManager
{
    private GameState _currentState;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public GameState NewGameState { get; }

        public OnStateChangedEventArgs(GameState newState)
        {
            NewGameState = newState;
        }
    }

    public GameStateManager()
    {
        ChangeCurrentState(GameState.NotStarted);
    }

    public void ChangeCurrentState(GameState newState)
    {
        _currentState = newState;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs(newState));
    }

    public GameState GetCurrentState => _currentState;

}
