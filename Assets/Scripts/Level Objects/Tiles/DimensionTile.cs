using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class DimensionTile : MonoBehaviour
{
    [SerializeField] private TilemapRenderer _tilemapRenderer;
    [SerializeField] private Dimension _visibleDimension;

    private DimensionStateHolder _dimensionStateHolder;
    
    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
        _dimensionStateHolder.OnDimensionChanged += OnDimensionChanged;
        OnDimensionChanged(_dimensionStateHolder.CurrentDimension);
    }

    private void OnDimensionChanged(Dimension dimension)
    {
        _tilemapRenderer.enabled = (dimension == _visibleDimension);
    }

    public void OnDestroy()
    {
        _dimensionStateHolder.OnDimensionChanged -= OnDimensionChanged;
    }
}
