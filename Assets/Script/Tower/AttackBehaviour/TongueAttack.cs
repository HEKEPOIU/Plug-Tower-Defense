using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Tower.AttackBehaviour
{
    public class TongueAttack : MonoBehaviour
    {
        SpriteSkin boneGroup;
        [SerializeField] Transform boneControler;
        [SerializeField] float speed = 1f;
        public static float Damage { get; set; }
        
        Enemy.Enemy targetToHit;
        [HideInInspector] public Transform target;
        float timer = 0;
    
        // Start is called before the first frame update
        void Start()
        {
            boneGroup = GetComponentInChildren<SpriteSkin>();
            targetToHit = target.GetComponent<Enemy.Enemy>();
            
            
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
            if (timer < 1)
            {
                Vector3 dir = target.position - boneGroup.rootBone.transform.position;
                boneGroup.rootBone.transform.right = dir.normalized;
                float distance = Mathf.Lerp(0 , dir.magnitude,timer);
                float offset = distance / 6 / 3;
                Vector3 newpPosition = new Vector3(offset, 0, 0);
                foreach (Transform bone in boneGroup.boneTransforms)
                {
                    if (bone.transform == boneGroup.rootBone)
                    {
                        continue;
                    }

                    bone.transform.localPosition = newpPosition;
                }

                Vector3 position = target.position;
                boneGroup.boneTransforms[^1].position = position;
                boneControler.position = position;
                timer += Time.deltaTime/speed;
            }

            else if (timer > 1)
            {
                targetToHit.TakeDamage(UnityEngine.Random.value <= Tower.CriticalRate ? (int)Damage*2 : (int)Damage);
                Destroy(gameObject);
            }
        }
    }
}
