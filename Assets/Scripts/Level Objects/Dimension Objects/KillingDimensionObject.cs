using UnityEngine;

public class KillingDimensionObject : BaseDimensionObject
{
    protected override void ProcessTriggeredByPlayer(PlayerMover playerMover)
    {
        Debug.Log("Calling triggered player mover");
        if (playerMover.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.DecreaseHealth(1);
        }
    }
}
