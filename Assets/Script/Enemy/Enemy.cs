using System.Collections;
using System.Linq;
using Manager;
using SpawnSystem;
using Tower;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [HideInInspector] public Transform target;
        [HideInInspector] public Path path;
        [HideInInspector] public Transform end;
        [HideInInspector] public Transform spawnPoint;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Slider hpBar;
        [SerializeField] int dropMoney;
        float speed = 2;
        public float baseSpeed;
        public int hp = 10;
        public static bool isSlow;
        bool moveable= false;

        void Start()
        {
            hp *= Mathf.FloorToInt(1 + (WaveManager.CurrentWave / 5) * 1.5f);
            hpBar.maxValue = hp;
            hpBar.value = hp;
            baseSpeed = speed;
            
            if (TowerManager.Instance.isAttributeTriggle[2].Value)
            {
                UpdateSpeed(1 - TowerManager.Towers.Count(tower => tower.towerAttributes == TowerAttributes.Water) * 0.1f);
                isSlow = true;
            }
            spriteRenderer.color = isSlow? Color.blue : Color.white;
        }

        void Update()
        {
            if (moveable == false)
            {
                moveable = Vector3.Distance(transform.position, spawnPoint.position) < .1f ? true: false;
                return;
            }
        
        
            if (Vector3.Distance(transform.position, end.position) < .1f)
            {
                ReachEnd();
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, target.position) < .1f)
            {
                target = path.NextPoint(target);
            }
            
        
        }
        
        public void UpdateSpeed(float slowRate)
        {
            speed = baseSpeed * slowRate;
            spriteRenderer.color = isSlow? Color.blue : Color.white;
        }

        void EnenyRed()
        {
            spriteRenderer.color = Color.red;
            
        }

        IEnumerator DamageEffect()
        {
            EnenyRed();
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = isSlow? Color.blue : Color.white;
        }
    
        public void TakeDamage(int damage)
        {
            hp -= damage;
            hpBar.value -= damage;
            StartCoroutine("DamageEffect");
            if (hp <= 0)
            {
                Death();
            }
        }

        void Death()
        {
            GameManager.Instance.Money += Mathf.FloorToInt(dropMoney * (1 + (WaveManager.CurrentWave / 5) * 1.5f));
            UIManager.Instance.MoneyChange(GameManager.Instance.Money);
            Destroy(gameObject);
        }
    
        void ReachEnd()
        {
            GameManager.Instance.Life--;
            UIManager.Instance.HpChange(GameManager.Instance.Life);
            MusicManager.Instance.PlayHpLoseAudio();

            if (GameManager.Instance.Life <= 0)
            {
                GameManager.Instance.Fail();
            }

            Death();
        }
    }
}
