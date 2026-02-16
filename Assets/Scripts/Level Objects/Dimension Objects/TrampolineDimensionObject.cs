using UnityEngine;

public class TrampolineDimensionObject : BaseDimensionObject
{

    [Header("Trampoline Configuration")]
    [SerializeField] private Dimension _trampolineActiveDimension;
    [SerializeField] private float _jumpForce = 32f;

    protected override void ProcessTriggeredByPlayer(PlayerMover playerMover)
    {
        if (_dimensionStateHolder.CurrentDimension != _trampolineActiveDimension)
            return;

        float trampolineVelocity = CalculateTrampolineVelocity();

        playerMover.LaunchFromTrampoline(trampolineVelocity);
    }

    private float CalculateTrampolineVelocity()
    {
        return Mathf.Sqrt(2f * Mathf.Abs(Physics2D.gravity.y) * _jumpForce);
    }
}
