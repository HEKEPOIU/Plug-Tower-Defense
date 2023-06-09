using System.Collections.Generic;
using System.Linq;
using Manager;
using Tower.AttackBehaviour;
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

        public static bool IsFighting = false;

        [ColorUsage(true, true)]
        public Color placeColor;
        
        public TowerAttributes towerAttributes;
    
        [Header("Tower基礎設定")]
        [SerializeField] protected float range = 15f;
        [Tooltip("Attack per second( 1/attackSpeed )")]
        [SerializeField] public float attackSpeed = 1f;
        [SerializeField] public float attackDamage = 10;
        public static float CriticalRate = 0f;
        float baseAttackDamage;
        float baseAttackSpeed;
        
        [SerializeField] GameObject fireEffect;
        
        protected Transform Target { get; set; }
        public bool IsAttack { get; set; } = false;
        float fireCountdown = 0f;
        protected void Start()
        {
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
            TowerManager.Towers.Add(this);
            baseAttackDamage = attackDamage;
            baseAttackSpeed = attackSpeed;


            EffectToggle(TowerManager.Instance.isAttributeTriggle[(int)towerAttributes].Value,
                (TowerManager.Towers.Where(tower => tower.towerAttributes == towerAttributes)).Count());
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
            if (Enemies.Count == 0 && IsFighting)
            {
                MusicManager.Instance.SwitchToBgm();
                IsFighting = false;
            }
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
        
        public void UpdateAttack(float newAttackDamage)
        {
            attackDamage = newAttackDamage;
            
        }


        public void EffectToggle(bool isOn ,int num)
        {
            switch (towerAttributes)
            {
                case TowerAttributes.Fire:
                    FireEffectToggle(isOn, num);
                    break;
                case TowerAttributes.Wind:
                    WindEffectToggle(isOn, num);
                    break;
                case TowerAttributes.Water:
                    WaterEffectToggle(isOn, num);
                    break;
                case TowerAttributes.Electromagnetic: 
                    ElectromagneticEffectToggle(isOn, num);
                    break;
                case TowerAttributes.None:
                    NoneEffectToggle(isOn, num);
                    break;

            }
        }
        
        protected virtual void Attack()
        {
        }
    
        void FireEffectToggle(bool isOn, int num)
        {
            fireEffect.SetActive(isOn);
            if (isOn)
            {
                FireEffect effect = fireEffect.GetComponent<FireEffect>();
                effect.UpdateDamage(baseAttackDamage * (0.3f+0.05f*num));
                effect.UpdateRadius(effect.baseRadius * (1 + 0.05f * num));
            }
        }
        void WindEffectToggle(bool isOn, int num)
        {
            if (isOn)
            {
                attackSpeed = baseAttackSpeed * (1 + num * 0.03f);
            }
            else
            {
                attackSpeed = baseAttackSpeed;
            }
        }
        void WaterEffectToggle(bool isOn, int num)
        {
            
            Enemy.Enemy.isSlow = isOn;
            foreach (GameObject enemy in Enemies)
            {
                if (enemy != null)
                {
                    Enemy.Enemy enemyScript = enemy.GetComponent<Enemy.Enemy>();
                    enemyScript.UpdateSpeed(isOn ? 1 - num*0.1f:1);
                }
            }
        }
        void ElectromagneticEffectToggle(bool isOn, int num)
        {
            if (isOn)
            {
                attackDamage = baseAttackDamage * (1 + num * 0.05f);
            }
            else
            {
                attackDamage = baseAttackDamage;
            }

        }
        void NoneEffectToggle(bool isOn, int num)
        {
            CriticalRate = isOn ? 0.1f * num : 0f;
        }
        
    
    
        void OnDrawGizmosSelected()
        {
            //forDebug draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        
        }
    }
}
