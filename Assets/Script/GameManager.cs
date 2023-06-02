using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    int money = 0;
    float currentTime = 0;
    float nextTime = 0;
    [SerializeField] float moneyJumpSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        MoneyJump(moneyJumpSpeed);
    }

    void MoneyJump(float speed,int moneyMult = 1)
    {
        if (!(currentTime > nextTime)) return;
        int openPlugins = uiManager.plugsToggleList.Count(item => item.isOn == true);
        money += openPlugins * moneyMult;
        uiManager.MoneyChange(money);
        nextTime = currentTime + speed;
    }
}
