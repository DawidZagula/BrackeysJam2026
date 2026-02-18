using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerVisual : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float _turnSpeed = 5f;

    //References
    private PlayerMover _playerMover;
    private SquashAndStretch _squashAndStretch;
    private Animator _animator;

    // run-time state
    private Coroutine _currentRotationCoroutine;
    private Action _onDeathAnimFinish;

    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    private void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }
    
    private void Awake()
    {
        _playerMover = GetComponentInParent<PlayerMover>();
        _squashAndStretch = GetComponentInParent<SquashAndStretch>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _dimensionStateHolder.OnDimensionChanged
            += DimensionStateHolder_OnDimensionChanged;
    }

    public void PlayAnimation(string stateName, Action onFinish = null)
    {
        _onDeathAnimFinish = onFinish;
        _animator.Play(stateName, 0);
    }

    //Called from animation Event
    public void CallOnDeathAnimFinish()
    {
        _onDeathAnimFinish?.Invoke();
    }

    private void DimensionStateHolder_OnDimensionChanged(Dimension dimension)
    {
        float targetZ = (dimension == Dimension.Goofy) ? 180f : 0f;
        _playerMover.SetUpsideDown(dimension == Dimension.Goofy);

        if (_currentRotationCoroutine != null)
            StopCoroutine(_currentRotationCoroutine);

        _currentRotationCoroutine = StartCoroutine(SmoothRotateZ(targetZ));
    }

    private IEnumerator SmoothRotateZ(float targetZ)
    {
        float startZRotation = _squashAndStretch != null
            ? _squashAndStretch.FlipAngle
            : transform.localEulerAngles.z;
        float t = 0f;

        if (startZRotation > 180f)
            startZRotation -= 360f;

        while (t < 1f)
        {
            t += Time.deltaTime * _turnSpeed;
            float z = Mathf.LerpAngle(startZRotation, targetZ, t);

            if (_squashAndStretch != null)
                _squashAndStretch.SetFlipAngle(z);
            else
                transform.localEulerAngles = new Vector3(0f, 0f, z);

            yield return null;
        }

        if (_squashAndStretch != null)
            _squashAndStretch.SetFlipAngle(targetZ);
        else
            transform.localEulerAngles = new Vector3(0f, 0f, targetZ);
    }
    private void OnDestroy()
    {
        if (_dimensionStateHolder != null)
        {
            _dimensionStateHolder.OnDimensionChanged
                -= DimensionStateHolder_OnDimensionChanged;
        }
    }
}