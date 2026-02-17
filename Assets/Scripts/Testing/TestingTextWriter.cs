using UnityEngine;
using UnityEngine.InputSystem;

public class TestingTextWriter : MonoBehaviour
{
    [SerializeField] private TextSequenceId _sequenceId;

    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            TextWriter.Instance.StartTypingSequence(_sequenceId);
        }
    }
}
