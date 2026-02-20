using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

/**
 * I'm not refactoring DimensionTileCollider because of lack of time.
 * DimensionTileCollider is only for spikes.
 * Here is a good use case for using [RequireComponent(typeof(TilemapCollider2D))]
 * with GetComponent.
 */
[RequireComponent(typeof(TilemapCollider2D))]
public class DimensionTileCollision : MonoBehaviour
{
    [SerializeField] private Dimension _visibleDimension; 

    private TilemapCollider2D _tilemapCollider2D;
    private DimensionStateHolder _dimensionStateHolder;
    
    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
        _dimensionStateHolder.OnDimensionChanged += OnDimensionChanged;
    }

    private void Awake()
    {
        _tilemapCollider2D = GetComponent<TilemapCollider2D>();
        OnDimensionChanged(_dimensionStateHolder.CurrentDimension);
    }
    
    private void OnDimensionChanged(Dimension dimension)
    {
        _tilemapCollider2D.enabled = _visibleDimension == dimension;
    }
}
