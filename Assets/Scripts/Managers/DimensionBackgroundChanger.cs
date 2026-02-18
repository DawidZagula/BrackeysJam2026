using UnityEngine;
using Zenject;

public class DimensionBackgroundChanger : MonoBehaviour
{
    [SerializeField] private Color _lavaBackgroundColour;
    [SerializeField] private Color _goofyBackgroundColour;

    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }

    private void Start()
    {
        _dimensionStateHolder.OnDimensionChanged += OnDimensionChanged;
    }

    private void OnDimensionChanged(Dimension obj)
    {
        switch (obj)
        {
            case Dimension.Lava:
                Camera.main.backgroundColor = _lavaBackgroundColour;
                break;
            case Dimension.Goofy:
                Camera.main.backgroundColor= _goofyBackgroundColour;
                break;
        }
    }
}
