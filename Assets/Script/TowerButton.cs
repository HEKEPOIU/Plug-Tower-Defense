using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]GameObject tower;
    [SerializeField] int cost;
    [SerializeField] Text moneyText;
    // Start is called before the first frame update
    void Start()
    {
        moneyText.text = "<b>"+cost+"</b>";
    }
    
    public void OnClick()
    {
        if (GameManager.Instance.Money < cost) return;
        BuildManager.Instance.Tower = tower;
        BuildManager.Instance.Cost = cost;
        BuildManager.Instance.BuildReady();
    }
}
