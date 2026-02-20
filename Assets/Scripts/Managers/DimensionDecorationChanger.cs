using UnityEngine;
using Zenject;

public class DimensionDecorationChanger : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Dimension _initialDimension;

    
    [Header("References")]
    [SerializeField] private Transform _lavaDecorationsHolder;
    [SerializeField] private Transform _goofyDecorationsHolder;



    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
        _dimensionStateHolder.OnDimensionChanged += OnDimensionChanged;
    }

    private void Start()
    {
        ToggleDimension(_initialDimension);
    }

    private void OnDimensionChanged(Dimension obj)
    {
        ToggleDimension(obj);
    }

    private void ToggleDimension(Dimension newDimension)
    {
        switch (newDimension)
        {
            case Dimension.Lava:
                {
                    _lavaDecorationsHolder.gameObject.SetActive(true);
                    _goofyDecorationsHolder.gameObject.SetActive(false);
                }
                break;
            case Dimension.Goofy:
                {
                    _lavaDecorationsHolder?.gameObject.SetActive(false);
                    _goofyDecorationsHolder.gameObject?.SetActive(true);
                }
                break;
        }
    }
}
