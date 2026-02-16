using System;
using System.Collections;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float _turnSpeed = 5f;

    //References
    private PlayerMover _playerMover;

    // run-time state
    private Coroutine _currentRotationCoroutine;

    private void Awake()
    {
        _playerMover = GetComponentInParent<PlayerMover>();
    }

    private void Start()
    {
        DimensionStateHolder.Instance.OnDimensionChanged
            += DimensionStateHolder_OnDimensionChanged;
    }

    private void DimensionStateHolder_OnDimensionChanged(object sender, DimensionStateHolder.OnDimensionChangedEventArgs e)
    {
        float targetZ = (e.newDimension == Dimension.Goofy) ? 180f : 0f;
        _playerMover.SetUpsideDown(e.newDimension == Dimension.Goofy);

        if (_currentRotationCoroutine != null)
            StopCoroutine(_currentRotationCoroutine);

        _currentRotationCoroutine = StartCoroutine(SmoothRotateZ(targetZ));
    }

    private IEnumerator SmoothRotateZ(float targetZ)
    {
        float startZRotation = transform.localEulerAngles.z;
        float t = 0f;

        if (startZRotation > 180f) 
            startZRotation -= 360f;

        while (t < 1f)
        {
            t += Time.deltaTime * _turnSpeed;
            float z = Mathf.LerpAngle(startZRotation, targetZ, t);
            transform.localEulerAngles = new Vector3(0f, 0f, z);
            yield return null;
        }

        transform.localEulerAngles = new Vector3(0f, 0f, targetZ);
    }

    private void OnDestroy()
    {
        if (DimensionStateHolder.Instance != null)
        {
            DimensionStateHolder.Instance.OnDimensionChanged
                -= DimensionStateHolder_OnDimensionChanged;
        }
    }
}
