using Tower.AttackBehaviour;
using Unity.Mathematics;
using UnityEngine;

namespace Tower.FloorDamage_Tower
{
    public class FloorDamageTower : Tower
    {
        [Header("地板塔設定")]
        public GameObject bulletPrefab;
        [SerializeField] float duration = 3.0f;
        [SerializeField] float radius = 1.2f;
        [SerializeField] float damageTime = 1.0f;
        void Start()
        {
            UpdateAttack(attackDamage);
            FloorBullet.Duration = duration;
            FloorBullet.Radius = radius;
            FloorBullet.DamageTime = damageTime;
            base.Start();
        }
        
        void UpdateAttack(float newAttackDamage)
        {
            FloorBullet.Damage = newAttackDamage;
            base.UpdateAttack(newAttackDamage);
        }
        
        public void Fire()
        {
            if (Target!=null)
            {
                Instantiate(bulletPrefab, Target.position, quaternion.identity);
            }
        }
        
    }
}
