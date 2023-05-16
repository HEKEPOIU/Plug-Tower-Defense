using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("uiManerger")] [SerializeField] UIManager uiManager;
    int _money = 0;
    float _currentTime = 0;
    float _nextTime = 0;
    [SerializeField] float moneyJumpSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;

        MoneyJump(moneyJumpSpeed);
    }

    void MoneyJump(float speed,int moneyMult = 1)
    {
        if (!(_currentTime > _nextTime)) return;
        int openPlugnum = 0;
        foreach (var item in uiManager.plugsToggleList)
        {
            if (item.isOn == true)
            {
                openPlugnum++;
            }
        }
        _money += openPlugnum * moneyMult;
        uiManager.MoneyChange(_money.ToString());
        _nextTime = _currentTime + speed;
    }
}
