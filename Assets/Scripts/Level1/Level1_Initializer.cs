using UnityEngine;
using Zenject;

public class Level1_Initializer : MonoBehaviour
{
    private GameStateManager _gameStateManager;

    private bool _alreadyTriggered;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    private void Start()
    {
        _gameStateManager.ChangeCurrentState(GameState.Started);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_alreadyTriggered) { return; }
        if (collision.TryGetComponent(out Player player))
        {
            Debug.Log("Triggering");

            _alreadyTriggered = true;

            TextWriter.Instance.
                StartTypingSequence(TextSequenceId.Level_1_Intro, true);
        }
    }

}
