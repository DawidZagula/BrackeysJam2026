using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _exitConfirmationMenu;
    [SerializeField] private GameObject _backToMenuConfirmationMenu;

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

    void Start()
    {
        _isMainMenu = SceneManager.GetActiveScene().name == "GameMenuScene";

        if (_isMainMenu)
        {
            _mainMenu.SetActive(true);
            _startButton.SetActive(true);
            //_exitButton.SetActive(true); // for desktop release only
            _backToMenuButton.SetActive(false);

            if (_resumeButton != null) _resumeButton.SetActive(false);
        }
        else
        {
            _backToMenuButton.SetActive(true);
            _mainMenu.SetActive(false);
            _startButton.SetActive(false);
            _exitButton.SetActive(false);
            if (_resumeButton != null) _resumeButton.SetActive(true);
        }

        _optionMenu.SetActive(false);
        LoadSettings();
    }

    void Update()
    {
        if (!_isMainMenu && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_optionMenu.activeSelf)
                OnBackToMenu();
            else
                TogglePause();
        }
    }

    // ---- PAUSE ----
    public void TogglePause()
    {
        _isPaused = !_isPaused;
        _mainMenu.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
    }

    public void OnResumeButton()
    {
        _isPaused = false;
        _mainMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    // ---- NAVIGATION ----
    public void OnStartButton()
    {
        Time.timeScale = 1f;
        SceneLoader.ProcessLoadScene(GameScene.Level_1);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnBackToMainMenu()
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

    }

    public void OnBackToMenu()
    {
        if (_optionMenu.activeInHierarchy)
        {
            _optionMenu.SetActive(false);
        }

        _mainMenu.SetActive(true);
        SaveSettings();
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