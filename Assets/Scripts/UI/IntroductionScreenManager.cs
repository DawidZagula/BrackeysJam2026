using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class IntroductionScreenManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _IntroductionScreen;
    [SerializeField] private Button _startButton;

    private GameStateManager _gameStateManager;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }
    
    private void Start()
    {
        AssignButton();
    }

    private void AssignButton()
    {
        _startButton.onClick.AddListener(() =>
        {
            _IntroductionScreen.SetActive(false);
            _gameStateManager.ChangeCurrentState(GameState.Started);
        });
    }
}
