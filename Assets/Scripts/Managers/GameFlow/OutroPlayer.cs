using TMPro;
using UnityEngine;

public class OutroPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _targetField;

    private void Start()
    {
        TextWriter.Instance.StartTypingSequence
            (TextSequenceId.Outro, _targetField, false, FadeOutToCompleteGame);
    }

    private void FadeOutToCompleteGame()
    {
        SceneLoader.ProcessLoadScene(GameScene.MainMenu);
    }

}
