using UnityEngine;

public class OutroAnimEndTrigger : MonoBehaviour
{
    [SerializeField] private OutroPlayer _outroPlayer;

    public void TriggerTextDisplaySequenceOnOutroPlayer()
    {
        _outroPlayer.StartTextDisplaySequence();
    }
}
