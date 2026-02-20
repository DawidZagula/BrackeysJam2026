using UnityEngine;
using Zenject;

public class GameStateInitializer : MonoBehaviour
{
    [SerializeField] private GameState _gameStateAtStart;
    
    private GameStateManager _gameStateManager;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }


    private void Start()
    {
        _gameStateManager.ChangeCurrentState(_gameStateAtStart);
    }
}
