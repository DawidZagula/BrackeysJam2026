using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set; }

    public enum AudioName
    {
        Death,
        Jump,
        TrampolineJump,
        ChangeDimension,
        ChangeDimension2,
        CollectItem,
        TransitionThroughPortal,
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

    void Start()
    {
        _audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void PlaySound(AudioName name, float volume = 1f)
    {
        if (name == AudioName.ChangeDimension ||  name == AudioName.ChangeDimension2)
        {
            StopCurrentlyPlayedSound();
        }
     _audioSource.PlayOneShot(_enumNameAudioClipDictionary[name], volume);
    }
    public void StopCurrentlyPlayedSound() => _audioSource.Stop();
    public bool IsPlaying() => _audioSource.isPlaying;

    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }
}