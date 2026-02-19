using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _quitGameConfirmationMenu;
    [SerializeField] private GameObject _backToMenuConfirmationMenu;
    [SerializeField] private GameObject _backgroundPanel;

    [Header("Buttons - Show/Hide per scene")]
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _resumeButton;
    [SerializeField] private GameObject _backToMenuButton;
    [SerializeField] private GameObject _exitButton;

    [SerializeField] private GameObject _creditsButton;
    [SerializeField] private GameObject _backFromCreditsButton;

    [SerializeField] private GameObject _exitConfirmationButton;
    [SerializeField] private GameObject _confirmExitButton;
    [SerializeField] private GameObject _backFromExitButton;

    [Header("Audio Settings")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private AudioSource _musicSource;

    private bool _isPaused = false;
    private bool _isMainMenu;

    private GameStateManager _gameStateManager;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    void Start()
    {
        _isMainMenu = SceneManager.GetActiveScene().name == "GameMenuScene";

        if (_isMainMenu)
        {
            _mainMenu.SetActive(true);
            _startButton.SetActive(true);
            _creditsButton.SetActive(true);
            //_exitButton.SetActive(true); // for desktop release only
            _backToMenuButton.SetActive(false);

            if (_resumeButton != null) _resumeButton.SetActive(false);
        }
        else
        {
            _backToMenuButton.SetActive(true);
            _mainMenu.SetActive(false);
            _startButton.SetActive(false);
            _creditsButton.SetActive(false);
            _exitButton.SetActive(false);

            _backgroundPanel.SetActive(false);

            if (_resumeButton != null) _resumeButton.SetActive(true);
        }

        _optionMenu.SetActive(false);
        LoadSettings();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_optionMenu.activeSelf)
            {
                OnBackToMenu();
                return;
            }
            else if (_creditsMenu.activeSelf)
            {
                OnBackToMenuFromCredits();
                return;
            }
            else if (_backToMenuConfirmationMenu.activeSelf)
            {
                OnBackToPauseFromMenuExitConfirmation();
                return;
            }
            else if (_quitGameConfirmationMenu.activeSelf)
            {
                //OnBackToMenuFromQuitGameConfirmation();
                //return;
            }
            else if (!_isMainMenu)
            {
                TogglePause();
            }
        }
    }

    // ---- PAUSE ----
    public void TogglePause()
    {
        _isPaused = !_isPaused;

        _backgroundPanel.SetActive(_isPaused);
        if (_isPaused)
        {
            _gameStateManager.ChangeCurrentState(GameState.Paused);
        }
        else
        {
            _gameStateManager.ChangeCurrentState(GameState.Started);
        }

        _mainMenu.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;

    }

    public void OnResumeButton()
    {
        _isPaused = false;
        _backgroundPanel.SetActive(false);
        _mainMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // ---- NAVIGATION ----
    public void OnStartButton()
    {
        Time.timeScale = 1f;
        SceneLoader.ProcessLoadScene(GameScene.Level_1);
    }

    public void OnOpenQuitConfirmButton()
    {
        _mainMenu.SetActive(false);
        _quitGameConfirmationMenu.SetActive(true);
    }

    //For desktop release only
    //public void OnConfirmExitButton()
    //{
    //    Application.Quit();
    //}

    public void OnOpenBackToMainMenuConfirmation()
    {
        _mainMenu.SetActive(false);
        _backToMenuConfirmationMenu.SetActive(true);
    }

    public void OnConfirmBackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.ProcessLoadScene(GameScene.MainMenu);

    }

    // ---- OPTIONS ----
    public void OnOptionButton()
    {
        _mainMenu.SetActive(false);
        _optionMenu.SetActive(true);
    }

    public void OnCreditsButton()
    {
        _mainMenu.SetActive(false);
        _creditsMenu.SetActive(true);
    }

    public void OnBackToMenu()
    {
        _optionMenu.SetActive(false);

        _mainMenu.SetActive(true);
        SaveSettings();
    }

    public void OnBackToMenuFromCredits()
    {
        _creditsMenu.SetActive(false);

        _mainMenu.SetActive(true);
    }

    //For desktop release only
    //public void OnBackToMenuFromQuitGameConfirmation()
    //{
    //    _quitGameConfirmationMenu.SetActive(false);

    //    _mainMenu.SetActive(true);
    //}

    public void OnBackToPauseFromMenuExitConfirmation()
    {
        _backToMenuConfirmationMenu.SetActive(false);

        _mainMenu.SetActive(true);
    }

    // ---- AUDIO ----
    public void OnMusicVolumeChanged(float value)
    {
        if (MusicPlayer.Instance != null)
            MusicPlayer.Instance.SetVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);

        if (AudioPlayer.Instance != null)
            AudioPlayer.Instance.SetVolume(value);
    }

    void LoadSettings()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (_musicSlider != null) _musicSlider.value = musicVol;
        if (_sfxSlider != null) _sfxSlider.value = sfxVol;

        if (MusicPlayer.Instance != null)
            MusicPlayer.Instance.SetVolume(musicVol);
    }

    void SaveSettings()
    {
        if (_musicSlider != null)
            PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
        if (_sfxSlider != null)
            PlayerPrefs.SetFloat("SFXVolume", _sfxSlider.value);
        PlayerPrefs.Save();
    }

}