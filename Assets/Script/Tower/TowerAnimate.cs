using System;
using Tower.AttackBehaviour;
using Tower.Projectile_Tower;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tower
{
    public class TowerAnimate : MonoBehaviour
    {
        Animator animator;
        Tower tower;
        //Use event to separate animation and logic
        public UnityEvent onAnimateEnd;
        
        
        static readonly int IsFight = Animator.StringToHash("IsFight");

        void Awake()
        {
            animator = GetComponent<Animator>();
            
        }

        void Start()
        {
            tower = GetComponentInParent<Tower>();
            SetSpeed(tower.attackSpeed);
        }

        void Update()
        {
            animator.SetBool(IsFight,tower.IsAttack);
        }


        public void SetSpeed(float speed)
        {
            animator.speed = speed;
        }
        
        public void ShootAnimateEnd()
        {
            onAnimateEnd?.Invoke();
        }
    }
}
