using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Tower
{
    public enum TowerAttributes
    {
        Fire,
        Wind,
        Water,
        Electromagnetic,
        None
    }
    
    
    public class Tower : MonoBehaviour
    {
        //All Tower know all enemy, so we can use this to find nearest enemy.
        //avoid using FindGameObjectWithTag, FindGameObjectsWithTag, they are slow.
        public static List<GameObject> Enemies = new List<GameObject>();
        
        public TowerAttributes towerAttributes;
    
        [Header("Tower基礎設定")]
        [SerializeField] protected float range = 15f;
        [Tooltip("Attack per second( 1/attackSpeed )")]
        [SerializeField] public float attackSpeed = 1f;
        [SerializeField] public int attackDamage = 10;
        

        protected Transform Target { get; set; }
        public bool IsAttack { get; set; } = false;
        float fireCountdown = 0f;
        protected void Start()
        {
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
            TowerManager.Instance.Towers.Add(this);
        }
    


        void UpdateTarget()
        {
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in Enemies)
            {
            
                if (enemy == null)
                {
                    continue;
                }
                else
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (!(distanceToEnemy < shortestDistance)) continue;
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        
            if (nearestEnemy != null && shortestDistance <= range) Target = nearestEnemy.transform;
            else Target = null;

            Enemies.RemoveAll(item => item == null);
        }
    
        void Update()
        {

            if (Target == null)
            {
                IsAttack = false;
                return;
            }
        
            Vector3 dir = Target.position - transform.position;
            transform.localScale = dir.x > 0 ? new Vector3(-1,1,1) : new Vector3(1,1,1);
            
            fireCountdown -= Time.deltaTime;
            if (fireCountdown <= 0f)
            {
                Attack();
                fireCountdown = 1f / attackSpeed;
            }
            IsAttack = true;

        }
        
        public void UpdateAttack(int newAttackDamage)
        {
            attackDamage = newAttackDamage;
        }


        public void EffectToggle(bool isOn)
        {
            
        }
        
        protected virtual void Attack()
        {
        }

        
    
    
        void OnDrawGizmosSelected()
        {
            //forDebug draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        
        }
    }
}
