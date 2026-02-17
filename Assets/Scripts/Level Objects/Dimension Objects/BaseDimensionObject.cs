using UnityEngine;
using Zenject;

public abstract class BaseDimensionObject : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Dimension _visibilityDimension;
    [Space]
    [SerializeField] private bool _isColliderAlwaysActive;
    [SerializeField] private Dimension _colliderActiveDimension;
    [SerializeField] private bool _isAlwaysTrigger;
    [SerializeField] private Dimension _triggerDimension;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _visualRenderer;
    [SerializeField] private Collider2D _collider;

    [Header("Components for two-dimensional")]
    [SerializeField] private bool _hasDifferentVisuals;
    [SerializeField] private Sprite _lavaDimensionVisual;
    [SerializeField] private Sprite _goofyDimensionVisual;

    protected DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }
    
    private void Start()
    {
        _dimensionStateHolder.OnDimensionChanged
            += DimensionStateHolder_OnDimensionChanged;

        ToggleVisual(Dimension.Lava);
        if (!_isColliderAlwaysActive)
        {
            ToggleCollider(Dimension.Lava);
        }

        if (!_isAlwaysTrigger)
        {
            ToggleTrigger(Dimension.Lava);
        }
        else
        {
            _collider.isTrigger = true;
        }
    }

    private void DimensionStateHolder_OnDimensionChanged(Dimension dimension)
    {
        ToggleVisual(dimension);
        if (!_isColliderAlwaysActive)
        {
            ToggleCollider(dimension);
        }
        if (!_isAlwaysTrigger)
        {
            ToggleTrigger(dimension);
        }
    }

    private void ToggleVisual(Dimension newDimension)
    {
        if (_hasDifferentVisuals)
        {
            switch (newDimension)
            {
                case Dimension.Lava:
                    _visualRenderer.sprite = _lavaDimensionVisual;
                    break;

                case Dimension.Goofy:
                    _visualRenderer.sprite = _goofyDimensionVisual;
                    break;
            }
        }
        else
        {
            _visualRenderer.enabled = newDimension == _visibilityDimension;
        }
    }

    private void ToggleCollider(Dimension newDimension)
    {
        _collider.enabled = newDimension == _colliderActiveDimension;
    }

    private void ToggleTrigger(Dimension newDimension)
    {
        _collider.isTrigger = newDimension == _triggerDimension;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMover playerMover))
        {
            ProcessTriggeredByPlayer(playerMover);
        }
    }
    protected virtual void ProcessTriggeredByPlayer(PlayerMover playerMover)
    {
        //Do nothing by default
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If we decide to have some SFX/ VFX or other logic on collision
    }


    private void OnDestroy()
    {
        _dimensionStateHolder.OnDimensionChanged
            -= DimensionStateHolder_OnDimensionChanged;
    }
}
