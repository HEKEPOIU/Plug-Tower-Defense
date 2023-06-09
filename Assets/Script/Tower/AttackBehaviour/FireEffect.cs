using UnityEngine;

namespace Tower.AttackBehaviour
{
    public class FireEffect : MonoBehaviour
    {
        float timer;
        static float DamageTime { get; set; } = 0.5f;
        static float Damage { get; set; } = 4.0f;
        static float Radius { get; set; } = 3f;
        [HideInInspector] public float baseRadius = 3f;
        [SerializeField] ParticleSystem particle;
        void Start()
        {
            UpdateRadius(Radius);
        }
        void Update()
        {
            UpdateDamage(Damage);
            UpdateRadius(Radius);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, Radius, Vector2.up, 2.0f,
                    LayerMask.GetMask("Enemy"));

                foreach (RaycastHit2D hit in hits)
                {
                    //need to be change, it slow, but it work, and I don't have Time to fix it.
                    hit.transform.GetComponent<Enemy.Enemy>().TakeDamage(UnityEngine.Random.value <= Tower.CriticalRate ? (int)Damage*2 : (int)Damage);
                }

                timer = DamageTime;
            }
        }
        
        public void UpdateDamage(float newDamage)
        {
            Damage = newDamage;
        }

        public void UpdateRadius(float radius)
        {
            Radius = radius;
            transform.localScale = new Vector3(Radius, Radius, Radius);
            ParticleSystem.ShapeModule shape = particle.shape;
            shape.scale = new Vector3(Radius*2, Radius*2, Radius*2);
        }

        void OnDrawGizmosSelected()
        {
            //forDebug draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        
        }
        
    }
}
