using Tower.AttackBehaviour;
using UnityEngine;

namespace Tower.Projectile_Tower
{
    public class ProjectileTower : Tower
    {
        TowerAnimate towerAnimate;
        public Transform firePoint;
        public GameObject bulletPrefab;

        void Start()
        {
            towerAnimate = GetComponentInChildren<TowerAnimate>();
            UpdateAttack(attackDamage);
            base.Start();
        }
        
        protected override void Shoot()
        {
            towerAnimate.Play();
        }

        void UpdateAttack(int newAttackDamage)
        {
            BulletTrack.Damage = newAttackDamage;
            base.UpdateAttack(newAttackDamage);
        }

        protected override void StopAnimate()
        {
            towerAnimate.Stop();
        }
        public void InitBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<BulletTrack>().target = Target;
        }
        
    }
}
