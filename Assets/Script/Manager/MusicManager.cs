using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Manager
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get;private set; }

        [SerializeField] AudioMixer gameMixer;

        [Header("AudioSource")]
        [SerializeField] AudioSource uiAudioSource;
        [SerializeField] AudioSource plugAudioSource;
        [SerializeField] AudioSource nextWaveAudioSource;
        [SerializeField] AudioSource fightingAudioSource;
        [SerializeField] AudioSource bgmAudioSource;
        [SerializeField] AudioSource towerClickAndBuildSource;
        [SerializeField] AudioSource hpLoseAudioSource;
        [SerializeField] AudioSource towerAttackAudioSource;

        bool fading = false;
    
        [Header("AudioClip")]
        [SerializeField] AudioClip plugAudio;
        [SerializeField] AudioClip uiAudio;
        [SerializeField] AudioClip nextWaveAudio;
        [SerializeField] AudioClip buildTowerAudio;
        [SerializeField] AudioClip clickTowerIconAudio;
        [SerializeField] AudioClip hpLoseAudio;
        [SerializeField] AudioClip[] towerAttackAudio; 
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
        
        public void PlayTowerAttackAudio(int index)
        {
            towerAttackAudioSource.PlayOneShot(towerAttackAudio[index]);
        }
        
        public void PlayBuildTowerAudio()
        {
            towerClickAndBuildSource.PlayOneShot(buildTowerAudio);
        }        
        public void PlayClickTowerIconAudio()
        {
            towerClickAndBuildSource.PlayOneShot(clickTowerIconAudio);
        }
        
        public void SwitchToFighting()
        {
            bgmAudioSource.Stop();
            fightingAudioSource.Play();
        }
        
        public void PlayHpLoseAudio()
        {
            hpLoseAudioSource.PlayOneShot(hpLoseAudio);
        }
        
        public void SwitchToBgm()
        {
            fightingAudioSource.Stop();
            bgmAudioSource.Play();
        }

        public void MuteMusic(bool isMute)
        {
            int volume = isMute ? 0 : -80;
            gameMixer.SetFloat("Master",volume);
        }

    
    
    }
}
