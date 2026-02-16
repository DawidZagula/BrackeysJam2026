using UnityEngine;

public class KnockbackApplier : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float _knockbackDistance = 2f;
    [SerializeField] private float _knockbackDuration = 0.2f;
    [SerializeField, Range(0f, 80f)] private float _upwardAngleDegrees = 15f;
    public void ApplyKnockback(KnockbackReceiver receiver)
    {
        Vector2 direction = CalculateKnockbackDirection(receiver.transform.position);

        receiver.StartKnockback(direction, _knockbackDistance,_knockbackDuration);
    }

    private Vector2 CalculateKnockbackDirection(Vector2 targetPosition)
    {
        Vector2 baseDirection =
        (targetPosition - (Vector2)transform.position).normalized;

        float sign = baseDirection.x >= 0f ? 1f : -1f;

        float rotationAngle = _upwardAngleDegrees * sign;

        return Quaternion.AngleAxis(rotationAngle, Vector3.forward) * baseDirection;
    }

}
