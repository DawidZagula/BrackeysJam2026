using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneSequenceStarter : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool _shouldPlayAtStart;
    
    [Header("References")]
    [SerializeField] private DimensionSwapAbilityGainPoint _SequncePlayer;
    [SerializeField] private TextSequenceId _SequenceId;

   // private bool _isUsed;

    private void Start()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (CutsceneStateTracker.HasPlayedInScene(currentSceneIndex))
        {
            FadeTransitionManager.Instance.FadeIn();
            return;
        }

        if (_shouldPlayAtStart)
        {
            CutsceneStateTracker.MarkPlayed(currentSceneIndex);
            _SequncePlayer.SetOtherTextSequence(_SequenceId);
            _SequncePlayer.StartCutscene();
        }
    }

}
