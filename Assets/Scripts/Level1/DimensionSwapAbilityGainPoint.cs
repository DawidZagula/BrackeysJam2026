using System.Collections;
using UnityEngine;
using Zenject;

public class DimensionSwapAbilityGainPoint : MonoBehaviour
{

    //References
    [SerializeField] private Player _player;
    [SerializeField] private Transform _playerSpawnPosition;
    private GameStateManager _gameStateManager;
    private SpriteRenderer _visual;

    [Header("Configuration")]
    [SerializeField] private TextSequenceId _cutsceneTextSequence;

    //Runtime state
    private bool _isUsed;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    private void Awake()
    {
        _visual = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (RespawnSystem.Instance.GetIsAbilityLearnt())
        {
            _player.ToggleGravityChangeAvailable(true);
            
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isUsed) {return;}

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            _isUsed = true;

            RespawnSystem.Instance.SetIsAbilityLearnt(true);
            RespawnSystem.Instance.UpdateCurrentRespawnPoint(transform.position);

            _gameStateManager.ChangeCurrentState(GameState.Cutscene);

            FadeTransitionManager.Instance.FadeOut(StartCutsceneSequence);
        }
    }

    private void StartCutsceneSequence()
    {
        _visual.enabled = false;

        TextWriter.Instance.SetTextField(TextWriter.TextFieldsId.TopMiddle);

        TextWriter.Instance.
            StartTypingSequence(_cutsceneTextSequence, false, EndCutsceneSequence);
    }

    private void EndCutsceneSequence()
    {
        FadeTransitionManager.Instance.FadeIn(ResumeControlWithDimensionChangeAbility);
    }

    private void ResumeControlWithDimensionChangeAbility()
    {
        _player.ToggleGravityChangeAvailable(true);

        _gameStateManager.ChangeCurrentState(GameState.Started);

        StartCoroutine(CheckIfUsedAbilityRoutine());
    }

    private IEnumerator CheckIfUsedAbilityRoutine()
    {
        while (!_player.UsedGravityChangeFirstTime)
        {
            yield return new WaitForSeconds(1);
        }

        TextWriter.Instance.ClearText();
    }
}
