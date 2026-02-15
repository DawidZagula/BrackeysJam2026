using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _backToMainMenuButton;

    private void Start()
    {
        GameStateManager.Instance.OnStateChanged += GameStateManager_OnStateChanged;

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
