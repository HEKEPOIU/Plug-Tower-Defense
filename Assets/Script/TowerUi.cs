using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TowerUi : MonoBehaviour
{
    RectTransform rectTransform;
    Vector3 onPosition,offPosition;
    float height;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;
        height = rect.height;
        Vector3 position = transform.position;
        onPosition = new Vector3(position.x, position.y, position.z);
        offPosition = new Vector3(position.x, position.y - height, position.z);
    }

    public void OpenToggle(bool isOn)
    {
        transform.DOMove(isOn == true ? onPosition: offPosition, .5f).SetEase(Ease.InOutSine);
    }

    
}
