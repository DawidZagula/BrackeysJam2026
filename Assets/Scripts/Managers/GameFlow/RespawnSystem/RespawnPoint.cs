using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnPosition;

    private bool _isUsed;
    public int SpawnPointId { get; private set; }

    private void Start()
    {
        int index = transform.GetSiblingIndex();

        SpawnPointId = RespawnSystem.Instance.RegisterRespawnPoint(this, index);

        _isUsed = RespawnSystem.Instance.GetRespawnPointUsed(SpawnPointId);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isUsed) return;

        if (collision.TryGetComponent(out Player player))
        {
            RespawnSystem.Instance.UpdateCurrentRespawnPoint(_playerSpawnPosition.position);
            _isUsed = true;

            AudioPlayer.Instance.PlaySound(AudioPlayer.AudioName.SpawnPoint);

            RespawnSystem.Instance.SetRespawnPointUsed(SpawnPointId, true);

            TextWriter.Instance.StartTypingSequence(
                TextSequenceId.ReachedSpawnPoint,
                TextWriter.TextFieldsId.TopMiddle,
                true
            );
        }
    }

    public void Reset()
    {
        _isUsed = false;
    }
}
