using Manager;
using SpawnSystem;
using UI;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [HideInInspector] public Transform target;
        [HideInInspector] public Path path;
        [HideInInspector] public Transform end;
        [HideInInspector] public Transform spawnPoint;
        [SerializeField] int dropMoney;
        public float speed = 2;
        public int hp = 10;
        bool moveable= false;

        void Start()
        {
            hp *= Mathf.FloorToInt(1 + (WaveManager.CurrentWave / 5) * 1.5f);
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
    
        public void TakeDamage(int damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                Death();
            }
        }

        void Death()
        {
            GameManager.Instance.Money += Mathf.FloorToInt(dropMoney * (1 + (WaveManager.CurrentWave / 5) * 1.5f)) ;
            Destroy(gameObject);
        }
    
        void ReachEnd()
        {
            GameManager.Instance.Life--;
            UIManager.Instance.HpChange(GameManager.Instance.Life);

            if (GameManager.Instance.Life <= 0)
            {
                GameManager.Instance.Fail();
            }

            Death();
        }
    }
}
