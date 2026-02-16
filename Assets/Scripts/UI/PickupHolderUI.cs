using TMPro;
using UnityEngine;

public class PickupHolderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pickupCountText;

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
    }
}
