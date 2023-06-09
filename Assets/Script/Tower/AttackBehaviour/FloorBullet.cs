using System;
using UnityEngine;

namespace Tower.AttackBehaviour
{
    public class FloorBullet : MonoBehaviour
    {
        [HideInInspector] public Transform target;

        float timer;
        public static float DamageTime{ get; set; }
        
        public static float Damage { get; set; }
        public static float Duration { get; set; } = 2.0f;
        public static float Radius { get; set; } = 1.2f;

        void Start()
        {
            Destroy(gameObject, Duration);
            float scale = Radius / 0.4f;
            transform.localScale = new Vector3(scale, scale, scale);
        }

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer<=0)
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, Radius, Vector2.up, 2.0f, LayerMask.GetMask("Enemy"));

                foreach (RaycastHit2D hit in hits)
                {
                    //need to be change, it slow, but it work, and I don't have Time to fix it.
                    hit.transform.GetComponent<Enemy.Enemy>().TakeDamage(UnityEngine.Random.value <= Tower.CriticalRate ? (int)Damage*2 : (int)Damage);
                }

                timer = DamageTime;
            }
        }
        
        
        void OnDrawGizmosSelected()
        {
            //forDebug draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        
        }
    }
}
