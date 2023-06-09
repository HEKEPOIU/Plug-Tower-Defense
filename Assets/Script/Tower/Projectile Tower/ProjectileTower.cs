using Tower.AttackBehaviour;
using UnityEngine;

namespace Tower.Projectile_Tower
{
    public class ProjectileTower : Tower
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
            BulletTrack.Damage = newAttackDamage;
            base.UpdateAttack(newAttackDamage);
        }
        
        public void InitBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<BulletTrack>().target = Target;
        }
        
    }
}
