using UnityEngine;
using Zenject;

public class PlayingStateTrigger : MonoBehaviour
{
    private GameStateManager _gameStateManager;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    private void Start()
    {
        //For debugging
        _gameStateManager.ChangeCurrentState(GameState.Started);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_gameStateManager.GetCurrentState != GameState.Started)
        {
            _gameStateManager.ChangeCurrentState(GameState.Started);
            Debug.Log("Starting Game");
        }
    }
}
