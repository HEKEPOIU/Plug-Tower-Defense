using System;
using Tower.AttackBehaviour;
using Tower.Projectile_Tower;
using UnityEngine;

namespace Tower
{
    public class TowerAnimate : MonoBehaviour
    {
        Animator animator;
        ProjectileTower tower;
        
        
        static readonly int IsFight = Animator.StringToHash("IsFight");

        void Awake()
        {
            animator = GetComponent<Animator>();
            
        }

        void Start()
        {
            tower = GetComponentInParent<ProjectileTower>();
            SetSpeed(tower.attackSpeed);
        }

        public void Play()
        {
            animator.SetBool(IsFight,true);
        }

        public void Stop()
        {
            animator.SetBool(IsFight,false);
        }

        public void SetSpeed(float speed)
        {
            animator.speed = speed;
        }
        
        public void ShootAnimateEnd()
        {
            tower.InitBullet();
        }
    }
}
