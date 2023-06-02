using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public Path path;
    [HideInInspector] public Transform end;
    public float speed = 2;

    void Update()
    {
        print(Vector3.Distance(transform.position, end.position));
        if (Vector3.Distance(transform.position, end.position) < .1f)
        {
            Death();
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, target.position) < .1f)
        {
            target = path.NextPoint(target);
        }
        
        
    }
    
    void Death()
    {
        Destroy(gameObject);
    }
}
