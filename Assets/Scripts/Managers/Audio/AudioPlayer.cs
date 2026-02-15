using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set; }

    public enum AudioName
    {
        //To be added
    }

    [SerializeField] private List<AudioClip> _audioClipList = new List<AudioClip>();

    private Dictionary<AudioName, AudioClip> _enumNameAudioClipDictionary = new Dictionary<AudioName, AudioClip>();

    private AudioSource _audioSource;

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

    public void StopCurrentlyPlayedSound() => _audioSource.Stop();
    public bool IsPlaying() => _audioSource.isPlaying;
    public void PlaySound(AudioName name) => _audioSource.PlayOneShot(_enumNameAudioClipDictionary[name]);

}
