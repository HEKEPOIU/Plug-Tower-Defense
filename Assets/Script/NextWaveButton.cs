using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveButton : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            StartCoroutine(nameof(WaveDelay));
        });
    }
    
    IEnumerator WaveDelay()
    {
        button.interactable = false;
        yield return new WaitForSeconds(3);
        button.interactable = true;
    }
}
