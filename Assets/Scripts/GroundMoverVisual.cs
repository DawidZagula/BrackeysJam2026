using UnityEngine;
using Zenject;

public class GroundMoverVisual : MonoBehaviour
{
    [SerializeField] private GameObject _goofyDimensionVisual;

    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(GameStateManager gameStateManager, DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }

    private void Start()
    {
        _dimensionStateHolder.OnDimensionChanged += OnDimensionChanged;

        ToggleDimensionVisuals(false);
    }

    private void OnDimensionChanged(Dimension obj)
    {
        ToggleDimensionVisuals(obj == Dimension.Goofy);
    }

    private void ToggleDimensionVisuals(bool isGoofyDimension)
    {
        foreach (Transform child in transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (child.gameObject == _goofyDimensionVisual)
            {
                spriteRenderer.enabled = isGoofyDimension;
                continue;
            }

            spriteRenderer.enabled = !isGoofyDimension;
        }
    }

    private void OnDestroy()
    {
        _dimensionStateHolder.OnDimensionChanged 
            -= OnDimensionChanged;
    }

}
