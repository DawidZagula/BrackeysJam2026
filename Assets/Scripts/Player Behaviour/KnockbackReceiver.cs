using System.Collections;
using UnityEngine;

public class KnockbackReceiver : MonoBehaviour
{
    // components
    private Rigidbody2D _rigidbody;

    //Run-time state
    private Coroutine _knockbackCoroutine;

    //Property
    [field: SerializeField] public bool IsBeingKnockedBack { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void StartKnockback(Vector2 direction, float distance, float duration)
    {
        if (IsBeingKnockedBack)
            return;

        if (duration <= 0f)
            return;

        if (_knockbackCoroutine != null)
            StopCoroutine(_knockbackCoroutine);

        _knockbackCoroutine = StartCoroutine(
            KnockbackRoutine(direction, distance, duration)
        );
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float distance, float duration)
    {
        IsBeingKnockedBack = true;

        Vector2 velocity = direction * (distance / duration);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            _rigidbody.linearVelocity = velocity;
            yield return new WaitForFixedUpdate();
        }

        _rigidbody.linearVelocity = Vector2.zero;
        IsBeingKnockedBack = false;
    }
}
