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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Starting Game");
        _gameStateManager.ChangeCurrentState(GameState.Started);
    }
}
