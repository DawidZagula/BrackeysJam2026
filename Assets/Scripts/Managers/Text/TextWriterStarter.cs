using UnityEngine;
using Zenject;

public class TextWriterStarter : MonoBehaviour
{

    [SerializeField] private TextSequenceId _textSequenceId;

    private bool _alreadyTriggered;

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
