using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManerger : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Button musicButton;
    [SerializeField] Button downListToggleButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button closeButton;
    [SerializeField] Image plugPannal;
    bool isdownListVisible = true;
    bool isplugPannalVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        musicButton.onClick.AddListener(ToggleMusic);
        downListToggleButton.onClick.AddListener(ToggleDownlist);
        settingButton.onClick.AddListener(TogglePlugPannal);
        closeButton.onClick.AddListener(TogglePlugPannal);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ToggleMusic()
    {
        if (audioSource.mute) audioSource.mute = false;
        else audioSource.mute = true;
    }
    void ToggleDownlist()
    {
        if (isdownListVisible)
        {
            downListToggleButton.transform.DOMoveY(-140,.5f);
            isdownListVisible = false;
        }
        else 
        {
            downListToggleButton.transform.DOMoveY(0, .5f);
            isdownListVisible = true;
        }
    }
    void TogglePlugPannal()
    {
        if (isplugPannalVisible)
        {
            plugPannal.transform.DOScale(Vector3.zero, .5F);
            isplugPannalVisible = false;
        }
        else
        {
            plugPannal.transform.DOScale(Vector3.one, .5F);
            isplugPannalVisible = true;
        }
    }
}
