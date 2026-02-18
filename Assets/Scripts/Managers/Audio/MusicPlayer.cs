using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    public enum AudioName
    {
        MenuTheme
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
        switch (scene.name)
        {
            case "GameMenuScene":
                PlayMusic(AudioName.MenuTheme);
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

    public void StopPlaying() => _audioSource.Stop();

    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    public float GetVolume()
    {
        return _audioSource.volume;
    }
}