using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PickupHolderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pickupCountText;
    [SerializeField] private Image _pickupIcon;

    [SerializeField] private Sprite _lavaSprite;
    [SerializeField] private Sprite _goofySprite;

    private DimensionStateHolder _dimensionStateHolder;

    [Inject]
    public void Construct(DimensionStateHolder dimensionStateHolder)
    {
        _dimensionStateHolder = dimensionStateHolder;
    }

    private void Start()
    {
        SubscribeEvents();

        UpdateCountText();
    }

    private void SubscribeEvents()
    {
        PickupHolder.Instance.OnUsedPickup
            += PickupHolder_OnUsedPickup;
        PickupHolder.Instance.OnAddedPickup
            += PickupHolder_OnAddedPickup;
        _dimensionStateHolder.OnDimensionChanged 
            += DimensionStateHolder_OnDimensionChanged;
    }

    private void DimensionStateHolder_OnDimensionChanged(Dimension obj)
    {
        switch (obj)
        {
            case Dimension.Lava:
                _pickupIcon.sprite = _lavaSprite;

                break;
            case Dimension.Goofy:
                _pickupIcon.sprite = _goofySprite;

                break;
        }
    }

    private void PickupHolder_OnUsedPickup(object sender, System.EventArgs e)
    {
        UpdateCountText();
    }

    private void PickupHolder_OnAddedPickup(object sender, System.EventArgs e)
    {
        UpdateCountText();
    }

    private void UpdateCountText()
    {
        string newPickupCountText = (PickupHolder.Instance.CurrentPickupCount).ToString();
        _pickupCountText.text = newPickupCountText;
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    private void UnsubscribeEvents()
    {
        PickupHolder.Instance.OnUsedPickup
            -= PickupHolder_OnUsedPickup;
        PickupHolder.Instance.OnAddedPickup
            -= PickupHolder_OnAddedPickup;
        _dimensionStateHolder.OnDimensionChanged
            -= DimensionStateHolder_OnDimensionChanged;
    }
}
