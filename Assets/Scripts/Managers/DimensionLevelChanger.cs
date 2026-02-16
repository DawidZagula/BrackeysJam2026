using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class DimensionLevelChanger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TilemapRenderer _lavaDimensionTilemapRenderer;
    [SerializeField] private TilemapRenderer _goofyDimensionTilemapRenderer;

    private Dictionary<Dimension, TilemapRenderer> _dimensionTilemapMap;
    private TilemapRenderer _currentActiveTilemap;

    private DimensionStateHolder _dimensionStateHolder;
    
    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }
    
    private void Awake()
    {
        _dimensionTilemapMap = new Dictionary<Dimension, TilemapRenderer>
         {
           { Dimension.Lava, _lavaDimensionTilemapRenderer },
           { Dimension.Goofy, _goofyDimensionTilemapRenderer }
         };

        UpdateDisplayedTilemap(Dimension.Lava);
    }

    private void Start()
    {
        _dimensionStateHolder.OnDimensionChanged += DimensionStateHolder_OnDimensionChanged;

        _currentActiveTilemap = _lavaDimensionTilemapRenderer;
    }

    private void DimensionStateHolder_OnDimensionChanged(Dimension dimension)
    {
        switch (dimension)
        {
            case Dimension.Lava:
                UpdateDisplayedTilemap(Dimension.Lava);
                break;

            case Dimension.Goofy:
                UpdateDisplayedTilemap(Dimension.Goofy);
                break;
        }
    }

    private void UpdateDisplayedTilemap(Dimension newDimension)
    {
        if (_currentActiveTilemap is not null)
        {
            _currentActiveTilemap.enabled = false;
        }

        _currentActiveTilemap = _dimensionTilemapMap[newDimension];
        _currentActiveTilemap.enabled = true;
    }

    private void OnDestroy()
    {
        _dimensionStateHolder.OnDimensionChanged 
            -= DimensionStateHolder_OnDimensionChanged;
    }
}
