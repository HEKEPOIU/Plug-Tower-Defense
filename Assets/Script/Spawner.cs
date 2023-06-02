﻿using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float spawnTime = 1;
    [SerializeField] Path path;
    Enemy[] enemy;
    Transform[] spawnPoint;
    int maxEnemy = 1;
    int enmeyCount = 0;

    Transform[] target;
    
    void Start()
    {
        enemy = new Enemy[enemyPrefabs.Length];
        spawnPoint = new Transform[transform.childCount];
        target = new Transform[spawnPoint.Length];
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            spawnPoint[i] = transform.GetChild(i);
            target[i] = path.FindNearestPoint(spawnPoint[i].position);
        }
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemy[i] = enemyPrefabs[i].GetComponent<Enemy>();
            enemy[i].path = path;
            enemy[i].end = spawnPoint[0];
        }
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0 && maxEnemy >= enmeyCount)
        {
            spawnTime = 1;
            enmeyCount++;
            Spawn();
        }
    }

    void Spawn()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        int spawnIndex = Random.Range(0, spawnPoint.Length);
        enemy[index].target = target[spawnIndex];
        GameObject instantiate = Instantiate(enemyPrefabs[index],
            spawnPoint[spawnIndex].position, Quaternion.identity);
    }
}
