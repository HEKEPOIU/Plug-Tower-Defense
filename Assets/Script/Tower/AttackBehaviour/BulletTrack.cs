using System;
using UnityEngine;

namespace Tower.AttackBehaviour
{
    public class BulletTrack : MonoBehaviour
    {
        [HideInInspector] public Transform target;
        Enemy.Enemy targetToHit;
        public static int Damage { get; set; }
        [SerializeField] float speed = 10f;

        void Start()
        {
            if (target != null)
            {
                targetToHit = target.GetComponent<Enemy.Enemy>();
            }
        }

        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;
            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.up = dir.normalized;
        }
        void HitTarget()
        {
            targetToHit.TakeDamage(Damage);
            Destroy(gameObject);
        }
        
    }



}
