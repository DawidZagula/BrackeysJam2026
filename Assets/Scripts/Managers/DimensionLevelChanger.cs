using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DimensionLevelChanger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TilemapRenderer _lavaDimensionTilemapRenderer;
    [SerializeField] private TilemapRenderer _goofyDimensionTilemapRenderer;

    private Dictionary<Dimension, TilemapRenderer> _dimensionTilemapMap;
    private TilemapRenderer _currentActiveTilemap;

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
        DimensionStateHolder.Instance.OnDimensionChanged += DimensionStateHolder_OnDimensionChanged;

        _currentActiveTilemap = _lavaDimensionTilemapRenderer;
    }

    private void DimensionStateHolder_OnDimensionChanged(object sender, DimensionStateHolder.OnDimensionChangedEventArgs e)
    {
        switch (e.newDimension)
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
}
