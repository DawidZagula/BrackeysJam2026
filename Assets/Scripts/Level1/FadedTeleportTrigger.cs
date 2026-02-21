using System.Collections;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class FadedTeleportTrigger : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Vector3 _teleportTarget;
    [SerializeField] private float _fadedTime;

    [Header("References")]
    [SerializeField] private Transform _player;
    private GameStateManager _gameStateManager;

    private Coroutine _runningRoutine;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (_runningRoutine != null) { return; }

        if (collision.TryGetComponent(out Player player))
        {

            _gameStateManager.ChangeCurrentState(GameState.Cutscene);

            FadeTransitionManager.Instance.FadeOut(ProceedTeleportSequence);
        }

    }

    private void ProceedTeleportSequence()
    {
       _runningRoutine = StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        _player.GetComponent<PlayerMover>().StopAllMovement();
        TeleportPlayer();
        _player.GetComponent<PlayerMover>().EnableMovement();
        yield return new WaitForSeconds(_fadedTime);
        FadeTransitionManager.Instance.FadeIn(SetGameStateStarted);
    }

    private void TeleportPlayer()
    {
        _player.transform.position = _teleportTarget;
    }

    private void SetGameStateStarted()
    {
        _gameStateManager.ChangeCurrentState(GameState.Started);
        _runningRoutine = null;
    }
}
