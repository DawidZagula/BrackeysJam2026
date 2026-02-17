using System;
using UnityEngine;
using Zenject;

public class GravityChanger : MonoBehaviour
{
    public static GravityChanger Instance { get; private set; }


    private Vector2 _defaultGravityValue;

    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }

    private void Awake()
    {
        Instance = this;

        _defaultGravityValue = Physics2D.gravity;
    }


    private void Start()
    {
        _dimensionStateHolder.OnDimensionChanged
            += DimensionStateHolder_OnDimensionChanged;
    }

    private void DimensionStateHolder_OnDimensionChanged(Dimension dimension)
    {
        ChangeGravity(dimension);
    }

    private void OnDestroy()
    {
        Physics2D.gravity = _defaultGravityValue;

        _dimensionStateHolder.OnDimensionChanged
            -= DimensionStateHolder_OnDimensionChanged;
    }

    private void ChangeGravity(Dimension newDimension)
    {
        bool invertGravity = newDimension == Dimension.Goofy;

        Physics2D.gravity = -Physics2D.gravity;
    }

}
