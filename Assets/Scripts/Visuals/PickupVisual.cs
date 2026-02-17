using UnityEngine;
using Zenject;

public class PickupVisual : MonoBehaviour
{
    private Animator _animator;

    private const string LavaWorldAnimName = "lavaSlowDown_0_base";
    private const string GoofyWorldAnimName = "lavaSlowDown_Goofy_0_base";

    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _dimensionStateHolder.OnDimensionChanged 
            += DimensionStateHolder_OnDimensionChanged;
    }

    private void DimensionStateHolder_OnDimensionChanged(Dimension obj)
    {
        switch (obj)
        {
            case Dimension.Lava:
                _animator.Play(LavaWorldAnimName);
                break;
            case Dimension.Goofy:
                _animator.Play(GoofyWorldAnimName);
                break;
        }
    }

    private void OnDestroy()
    {
        _dimensionStateHolder.OnDimensionChanged 
            -= DimensionStateHolder_OnDimensionChanged;

    }
}
