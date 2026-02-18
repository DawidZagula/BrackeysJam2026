using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnPosition;
    
    private bool _isUsed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isUsed) { return; }

        if (collision.TryGetComponent(out Player player))
        {
            RespawnSystem.Instance.UpdateCurrentRespawnPoint(_playerSpawnPosition.position);
            _isUsed = true;
            Debug.Log("Changed respawn point");
        }
    }
}
