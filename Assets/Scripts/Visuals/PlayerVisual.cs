using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerVisual : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float _turnSpeed = 5f;


    [Header("References")]
    [SerializeField] private Transform _anchorTransform;
    private PlayerMover _playerMover;
    private SquashAndStretch _squashAndStretch;

    // run-time state
    private Coroutine _currentRotationCoroutine;

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
    }

    private void Start()
    {
        _dimensionStateHolder.OnDimensionChanged
            += DimensionStateHolder_OnDimensionChanged;
    }

    private void DimensionStateHolder_OnDimensionChanged(Dimension dimension)
    {
        _squashAndStretch.CanWork = false;

        float targetZ = (dimension == Dimension.Goofy) ? 180f : 0f;
        _playerMover.SetUpsideDown(dimension == Dimension.Goofy);

        if (_currentRotationCoroutine != null)
            StopCoroutine(_currentRotationCoroutine);

        _currentRotationCoroutine = StartCoroutine(SmoothRotateZ(targetZ));
    }

    private IEnumerator SmoothRotateZ(float targetZ)
    {
        _squashAndStretch.CanWork = false;

        _anchorTransform.localRotation = Quaternion.identity;
        _anchorTransform.localScale = Vector3.one;
        
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        float startZRotation = transform.localEulerAngles.z;
        if (startZRotation > 180f)
            startZRotation -= 360f;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * _turnSpeed;
            float z = Mathf.LerpAngle(startZRotation, targetZ, t);
            transform.localEulerAngles = new Vector3(0f, 0f, z);
            yield return null;
        }

        transform.localEulerAngles = new Vector3(0f, 0f, targetZ);
        if (targetZ == 0f)
        {
            _squashAndStretch.CanWork = true;
        }
   
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
