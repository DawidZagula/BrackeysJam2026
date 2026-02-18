using UnityEngine;
using Zenject;

public class LavaFreezeObject : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 5f;

    private LavaFreezer _lavaFreezer;
    
    [Inject]
    public void Construct(LavaFreezer lavaFreezer)
    {
        _lavaFreezer =  lavaFreezer;
    }   
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMover playerMover))
        {
            _lavaFreezer.FreezeLava(freezeDuration);
            Destroy(gameObject);
        }
    }
}
