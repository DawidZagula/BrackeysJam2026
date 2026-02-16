using UnityEngine;

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

    private void Start()
    {
        DimensionStateHolder.Instance.OnDimensionChanged
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
    }

    private void DimensionStateHolder_OnDimensionChanged(object sender, DimensionStateHolder.OnDimensionChangedEventArgs e)
    {
        ToggleVisual(e.newDimension);
        if (!_isColliderAlwaysActive)
        {
            ToggleCollider(e.newDimension);
        }
        if (!_isAlwaysTrigger)
        {
            ToggleTrigger(e.newDimension);
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
    protected abstract void ProcessTriggeredByPlayer(PlayerMover playerMover);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If we decide to have some SFX/ VFX or other logic on collision
    }


    private void OnDestroy()
    {
        DimensionStateHolder.Instance.OnDimensionChanged
            -= DimensionStateHolder_OnDimensionChanged;
    }
}
