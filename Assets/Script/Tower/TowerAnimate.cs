using System;
using Manager;
using Tower.AttackBehaviour;
using Tower.Projectile_Tower;
using Unity.VisualScripting;
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

            AddAttackDamageMusic();
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

        void AddAttackDamageMusic()
        {
            switch (tower.towerAttributes)
            {
                case TowerAttributes.Fire:
                    onAnimateEnd.AddListener(()=>MusicManager.Instance.PlayTowerAttackAudio(0));
                    break;
                case TowerAttributes.Wind:
                    onAnimateEnd.AddListener(()=>MusicManager.Instance.PlayTowerAttackAudio(1));
                    break;
                case TowerAttributes.Water:
                    onAnimateEnd.AddListener(()=>MusicManager.Instance.PlayTowerAttackAudio(2));
                    break;
                case TowerAttributes.Electromagnetic: 
                    onAnimateEnd.AddListener(()=>MusicManager.Instance.PlayTowerAttackAudio(3));
                    break;
                case TowerAttributes.None:
                    onAnimateEnd.AddListener(()=>MusicManager.Instance.PlayTowerAttackAudio(4));
                    break;
            }
        }
    }
}
