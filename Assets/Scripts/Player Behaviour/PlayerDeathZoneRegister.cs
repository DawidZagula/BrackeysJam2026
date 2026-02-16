using UnityEngine;
using Zenject;

public class PlayerDeathZoneRegister : MonoBehaviour
{
    private GameStateManager _gameStateManager;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }
    
    public void ProceedDeath()
    {
        //TO IMPLEMENT: Transition
        _gameStateManager.ChangeCurrentState(GameState.GameOver);
    }


}
