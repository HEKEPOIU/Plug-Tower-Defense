using System.Collections;
using Manager;
using SpawnSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NextWaveButton : MonoBehaviour
    {
        Button button;
        [SerializeField] WaveManager waveManager;

        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(async () =>
            {
                MusicManager.Instance.SwitchToFighting();
                Tower.Tower.IsFighting = true;
                button.interactable = false;
                await waveManager.StartWave();
                button.interactable = true;
                
            });
        }
        
    }
}
