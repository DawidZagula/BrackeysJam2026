using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CameraShaker : MonoBehaviour
{
    private CinemachineImpulseSource _inputSource;
    public static CameraShaker Instance { get; private set; }

    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }

    private void Awake()
    {
        Instance = this;

        _inputSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        _dimensionStateHolder.OnDimensionChanged 
            += DimensionStateHolder_OnDimensionChanged;
    }

    private void DimensionStateHolder_OnDimensionChanged(Dimension obj)
    {
        ShakeCamera();
    }

    private void ShakeCamera()
    {
        const float baseShakeForce = 1;
        _inputSource.GenerateImpulseWithForce(baseShakeForce);
    }

    private void OnDestroy()
    {
        _dimensionStateHolder.OnDimensionChanged
          -= DimensionStateHolder_OnDimensionChanged;
    }
}
