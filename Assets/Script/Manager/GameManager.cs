using System.Linq;
using SpawnSystem;
using UI;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public int Money{ get; set; }
        public int Life { get; set; }
        float currentTime = 0;
        float nextTime = 0;
        [SerializeField] float moneyJumpSpeed = 1;
        // Start is called before the first frame update
        void Awake()
        {
            if (Instance !=null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Money = 100;
            Life = 10;
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
            int openPlugins = UIManager.Instance.plugsToggleList.Count(item => item.isOn == false);
            Money += openPlugins * moneyMult;
            UIManager.Instance.MoneyChange(Money);
            nextTime = currentTime + speed;
        }
    
        public void Fail()
        {
            UIManager.Instance.ToggleLostPanel();

        }

        public void GameReset()
        {
            WaveManager.CurrentWave--;
            foreach (GameObject obj in Tower.Tower.Enemies)
            {
                Destroy(obj);
            }
            MusicManager.Instance.SwitchToBgm();
            UIManager.Instance.ToggleLostPanel();
            Tower.Tower.Enemies.Clear();
        }
    }
}
