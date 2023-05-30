using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [HideInInspector] public Path path;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 2);
        if (Vector3.Distance(transform.position, target.position) < .1f)
        {
            target = path.NextPoint(target);
        }
    }
}
