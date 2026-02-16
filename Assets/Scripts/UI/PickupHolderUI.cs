using TMPro;
using UnityEngine;

public class PickupHolderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pickupCountText;

    private void Start()
    {
        PickupHolder.Instance.OnUsedPickup 
            += PickupHolder_OnUsedPickup;

        UpdateCountText();
    }

    private void PickupHolder_OnUsedPickup(object sender, System.EventArgs e)
    {
        UpdateCountText();
    }

    private void UpdateCountText()
    {
        string newPickupCountText = (PickupHolder.Instance.CurrentPickupCount).ToString();
        _pickupCountText.text = newPickupCountText;
    }
}
