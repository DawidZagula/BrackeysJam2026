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
        _gameStateManager.OnStateChanged += GameStateManager_OnStateChanged;

        AssignButtons();
    }

    private void GameStateManager_OnStateChanged(object sender, GameStateManager.OnStateChangedEventArgs e)
    {
        if (e.NewGameState == GameState.GameOver)
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
            // No main menu yet
        });
    }
}
