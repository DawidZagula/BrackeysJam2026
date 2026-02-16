using UnityEngine;

[RequireComponent(typeof(KnockbackApplier))]
public class KnockbackDimensionObject : BaseDimensionObject
{
    private KnockbackApplier _knockbackApplier;

    private void Awake()
    {
        _knockbackApplier = GetComponent<KnockbackApplier>();
    }

    protected override void ProcessTriggeredByPlayer(PlayerMover playerMover)
    {
        if (playerMover.TryGetComponent(out KnockbackReceiver receiver))
        {
            _knockbackApplier.ApplyKnockback(receiver);
            return;
        }
    }

}
