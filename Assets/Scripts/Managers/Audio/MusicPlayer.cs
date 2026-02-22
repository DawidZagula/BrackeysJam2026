using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    public enum AudioName
    {
        MenuTheme,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5
    }

    [SerializeField] private List<AudioClip> _audioClipList = new List<AudioClip>();
    private AudioSource _audioSource;
    private Dictionary<AudioName, AudioClip> _enumNameAudioClipDictionary = new Dictionary<AudioName, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();

        foreach (AudioName audioName in System.Enum.GetValues(typeof(AudioName)))
        {
            _enumNameAudioClipDictionary[audioName] = _audioClipList[(int)audioName];
        }
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0:
                PlayMusic(AudioName.MenuTheme);
                break;
            case 1:
                PlayMusic(AudioName.Level1);
                break;
            case 2:
                PlayMusic(AudioName.Level2);
                break;
            case 3:
                PlayMusic(AudioName.Level3);
                break;
            case 4:
                PlayMusic(AudioName.Level4);
                break;
            case 5:
                PlayMusic(AudioName.Level5);
                break;
            case 6:
                StopPlaying();
                break;
        }
    }

    public void PlayMusic(AudioName name)
    {
        if (_audioSource.clip != _enumNameAudioClipDictionary[name] || !_audioSource.isPlaying)
        {
            _audioSource.clip = _enumNameAudioClipDictionary[name];
            _audioSource.Play();
        }
    }

    private void StopPlaying() => _audioSource.Stop();

    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    public float GetVolume()
    {
        return _audioSource.volume;
    }
}