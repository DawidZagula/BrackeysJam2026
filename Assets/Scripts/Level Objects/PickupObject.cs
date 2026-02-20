using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMover playerMover))
        {
            PickupHolder.Instance.AddPickup();

            AudioPlayer.Instance.PlaySound(AudioPlayer.AudioName.CollectItem);

            Destroy(gameObject);
        }
    }
}
