using System;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get;private set; }

    [SerializeField] AudioMixer gameMixer;

    [Header("AudioSource")]
    [SerializeField] AudioSource uiAudioSource;
    [SerializeField] AudioSource plugAudioSource;
    [SerializeField] AudioSource nextWaveAudioSource;
    
    [Header("AudioClip")]
    [SerializeField] AudioClip plugAudio;
    [SerializeField] AudioClip uiAudio;
    [SerializeField] AudioClip nextWaveAudio;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void PlayUiAudio()
    {
        uiAudioSource.PlayOneShot(uiAudio);
    }
    public void PlayPlugAudio()
    {
        plugAudioSource.PlayOneShot(plugAudio);
    }
    public void PlayNextWaveAudio()
    {
        nextWaveAudioSource.PlayOneShot(nextWaveAudio);
    }

    public void MuteMusic(bool isMute)
    {
        int volume = isMute ? 0 : -80;
        gameMixer.SetFloat("Master",volume);
    }
    
    
    
}
