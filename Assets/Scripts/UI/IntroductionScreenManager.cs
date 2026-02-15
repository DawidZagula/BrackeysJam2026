using UnityEngine;
using UnityEngine.UI;

public class IntroductionScreenManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _IntroductionScreen;
    [SerializeField] private Button _startButton;

    private void Start()
    {
        AssignButton();
    }

    private void AssignButton()
    {
        _startButton.onClick.AddListener(() =>
        {
            _IntroductionScreen.SetActive(false);
            GameStateManager.Instance.ChangeCurrentState(GameState.Started);
        });
    }
}
