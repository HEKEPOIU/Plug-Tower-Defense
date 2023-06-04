using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform target;
    public float range = 15f;
    public static GameObject[] Enemies;

    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in Enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (!(distanceToEnemy < shortestDistance)) continue;
            shortestDistance = distanceToEnemy;
            nearestEnemy = enemy;

        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }

    }
    
    void Update()
    {
        //forDebug
        Gizmos.DrawSphere(transform.position, range);
    }
}
