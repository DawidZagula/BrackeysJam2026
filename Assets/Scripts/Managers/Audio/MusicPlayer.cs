using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    public enum AudioName
    {
        //To be added
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

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //Waiting for music files
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

}
