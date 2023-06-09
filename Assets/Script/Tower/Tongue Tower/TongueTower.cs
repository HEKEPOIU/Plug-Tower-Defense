using Tower.AttackBehaviour;
using UnityEngine;

namespace Tower.Tongue_Tower
{
    public class TongueTower : Tower
    {
        public Transform firePoint;
        public GameObject bulletPrefab;

        void Start()
        {
            UpdateAttack(attackDamage);
            base.Start();
        }
        
        void UpdateAttack(float newAttackDamage)
        {
            TongueAttack.Damage = newAttackDamage;
            base.UpdateAttack(newAttackDamage);
        }
        
        protected override void Attack()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<TongueAttack>().target = Target;
        }
    }
}
