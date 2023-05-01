using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManerger : MonoBehaviour
{
    [SerializeField] UIManerger uiManerger;
    int money = 0;
    float currentTime = 0;
    float nextTime = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime>nextTime)
        {
            money += WebManerger.Instance.TapoIP.Count * 1;
            uiManerger.MoneyChange(money.ToString());
            nextTime = currentTime+1;
        }
    }
}
