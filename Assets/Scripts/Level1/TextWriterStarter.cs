using UnityEngine;
using Zenject;

public class TextWriterStarter : MonoBehaviour
{
    private GameStateManager _gameStateManager;

    [SerializeField] private TextSequenceId _textSequenceId;

    private bool _alreadyTriggered;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_alreadyTriggered) { return; }
        if (collision.TryGetComponent(out Player player))
        {
            _alreadyTriggered = true;

            TextWriter.Instance.
                StartTypingSequence(_textSequenceId, true);
        }
    }

}
