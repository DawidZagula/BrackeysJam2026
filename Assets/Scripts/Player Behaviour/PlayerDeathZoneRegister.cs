using UnityEngine;

public class PlayerDeathZoneRegister : MonoBehaviour
{
    public void ProceedDeath()
    {
        //TO IMPLEMENT: Transition
        GameStateManager.Instance.ChangeCurrentState(GameState.GameOver);
    }


}
