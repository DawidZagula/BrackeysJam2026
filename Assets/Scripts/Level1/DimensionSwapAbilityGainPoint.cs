using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class DimensionSwapAbilityGainPoint : MonoBehaviour
{
    private enum GainPointType
    {
        GravityAbility,
        Artefact,
        PureCutscene
    }

    [Header("References")]
    [SerializeField] private Player _player;
    [SerializeField] private Transform _playerSpawnPosition;
    [SerializeField] private Transform _teleportToArtefact;
    [SerializeField] private Transform _trueGravityChangeGainer;
    private GameStateManager _gameStateManager;
    private SpriteRenderer _visual;

    [Header("Configuration")]
    [SerializeField] private TextSequenceId _gravityAbilityTextSequence;
    [SerializeField] private TextSequenceId _artefactTakenTextSequence;
    [SerializeField] private TextSequenceId _otherTextSequence;
    [SerializeField] private GainPointType _gainPointType;

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
        switch (_gainPointType)
        {
            case GainPointType.GravityAbility:
                UpdateGravityChangeRelated();
                break;

            case GainPointType.Artefact:
                UpdateArtifactRelated();
                break;
        }
    }

    private void UpdateGravityChangeRelated()
    {
        if (RespawnSystem.Instance.GetIsAbilityLearnt())
        {
            _player.ToggleGravityChangeAvailable(true);

            Destroy(gameObject);
        }
        else
        {
            if (gameObject.activeInHierarchy && !RespawnSystem.Instance.GetIsArtifactTaken())
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void UpdateArtifactRelated()
    {
        if (RespawnSystem.Instance.GetIsArtifactTaken())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isUsed) { return; }

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            _isUsed = true;

            _gameStateManager.ChangeCurrentState(GameState.Cutscene);

            if (_gainPointType == GainPointType.GravityAbility)
            {
                FadeTransitionManager.Instance.FadeOut(StartGravityGainCutsceneSequence);
            }
            else if (_gainPointType == GainPointType.Artefact)
            {
                FadeTransitionManager.Instance.FadeOut(StartArtifactTakenCutsceneSequence);
            }
            else
            {
                FadeTransitionManager.Instance.FadeOut(StartCutscene);
            }
        }
    }   

    private void StartGravityGainCutsceneSequence()
    {
        _visual.enabled = false;

        RespawnSystem.Instance.SetIsAbilityLearnt(true);
        RespawnSystem.Instance.UpdateCurrentRespawnPoint(transform.position);

        _player.GetComponent<PlayerMover>().StopAllMovement();


        TextWriter.Instance.
            StartTypingSequence
            (_gravityAbilityTextSequence, TextWriter.TextFieldsId.TopMiddle, false, EndGravityChangeGainCutsceneSequence);
    }

    private void EndGravityChangeGainCutsceneSequence()
    {
        _player.GetComponent<PlayerMover>().EnableMovement();
        FadeTransitionManager.Instance.FadeIn(ResumeControlWithDimensionChangeAbility);
    }

    private void ResumeControlWithDimensionChangeAbility()
    {
        _player.ToggleGravityChangeAvailable(true);

        ResumeControl();

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

    private void StartArtifactTakenCutsceneSequence()
    {
        _visual.enabled = false;
        _teleportToArtefact.gameObject.SetActive(false);
        _trueGravityChangeGainer.gameObject.SetActive(true);

        RespawnSystem.Instance.SetIsArtifactTaken(true);
        RespawnSystem.Instance.UpdateCurrentRespawnPoint(_playerSpawnPosition.position);

        _player.GetComponent<PlayerMover>().StopAllMovement();

        _player.transform.position = _playerSpawnPosition.position;

        TextWriter.Instance.
         StartTypingSequence
         (
            _artefactTakenTextSequence,
         TextWriter.TextFieldsId.TopMiddle,
         true,
         EndCutsceneSequence
         );
    }

    private void StartCutscene()
    {
        TextWriter.Instance.
     StartTypingSequence
     (_otherTextSequence, TextWriter.TextFieldsId.TopMiddle, true, EndCutsceneSequence);
    }

    private void EndCutsceneSequence()
    {
        _player.GetComponent<PlayerMover>().EnableMovement();
        FadeTransitionManager.Instance.FadeIn(ResumeControl);
    }

    private void ResumeControl()
    {
        _gameStateManager.ChangeCurrentState(GameState.Started);
    }
}
