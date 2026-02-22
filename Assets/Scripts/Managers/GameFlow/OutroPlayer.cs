using TMPro;
using UnityEngine;

public class OutroPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _targetTextField;
    [SerializeField] private Animator _outroAnimator;

    private void Start()
    {
        FadeTransitionManager.Instance.FadeIn(StartAnimationSequence);
               
    }

    private void StartAnimationSequence()
    {
        AudioPlayer.Instance.PlaySound(AudioPlayer.AudioName.EndingSound);
        _outroAnimator.Play("OutroBase");
    }


    public void StartTextDisplaySequence()
    {
        TextWriter.Instance.StartTypingSequence
            (TextSequenceId.Outro, _targetTextField, false, FadeOutToCompleteGame);
    }

    private void FadeOutToCompleteGame()
    {
        SceneLoader.ProcessLoadScene(GameScene.MainMenu);
    }

}
