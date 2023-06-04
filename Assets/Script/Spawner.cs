using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Path path;
    [SerializeField] Transform endPoint;
    Enemy[] enemy;
    [SerializeField] float waveSpawnTime;
    Transform[] spawnPoint;
    public int MaxEnemy{get; set;}

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
            enemy[i].end = endPoint;
        }
    }
    
    public async void SpawnWave()
    {
        for (int i = 0; i < MaxEnemy; i++)
        {
            Spawn();
            await Task.Delay((int)(waveSpawnTime * 1000 / MaxEnemy));
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
