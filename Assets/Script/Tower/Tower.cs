using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    public class Tower : MonoBehaviour
    {
        //All Tower know all enemy, so we can use this to find nearest enemy.
        //avoid using FindGameObjectWithTag, FindGameObjectsWithTag, they are slow.
        public static List<GameObject> Enemies = new List<GameObject>();
    
    
        [Header("Tower基礎設定")]
        [SerializeField] protected float range = 15f;
        [Tooltip("Attack per second( 1/attackSpeed )")]
        [SerializeField] public float attackSpeed = 1f;
        [SerializeField] public int attackDamage = 10;

        public Transform Target { get; set; }
        float fireCountdown = 0f;
        protected void Start()
        {
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
            print("base tower trigger");
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
                StopAnimate();
                return;
            }
        
            Vector3 dir = Target.position - transform.position;
            transform.localScale = dir.x > 0 ? new Vector3(-1,1,1) : new Vector3(1,1,1);


            if (fireCountdown <= 0)
            {
                Shoot();
                fireCountdown = 1/attackSpeed;
            }
            fireCountdown -= Time.deltaTime;

        }
        
        public void UpdateAttack(int newAttackDamage)
        {
            attackDamage = newAttackDamage;
        }

    
    
        /// <summary>
        /// Shoot enemy, depend on tower type, override this method.
        /// </summary>
        protected virtual void Shoot()
        {
        
        }

        protected virtual void StopAnimate()
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
