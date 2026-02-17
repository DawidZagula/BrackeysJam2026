using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _backToMainMenuButton;

    private GameStateManager _gameStateManager;
    
    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }
    
    private void Start()
    {
        _gameStateManager.OnStateChanged += OnStateChanged;

        AssignButtons();
    }

    private void OnStateChanged(GameState gameState)
    {
        if (gameState == GameState.GameOver)
        {
            _gameOverScreen.SetActive(true);
        }
    }

    private void AssignButtons()
    {
        _retryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        });

        _backToMainMenuButton.onClick.AddListener(() =>
        {
            //
        });
    }
}
