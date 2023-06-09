using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace SpawnSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] GameObject[] enemyPrefabs;
        [SerializeField] Path path;
        [SerializeField] Transform endPoint;
        [SerializeField] float waveSpawnTime;
        [HideInInspector] public Transform preGenerate;

        public UnityEvent levelEnd;
        static Transform parent;
        Enemy.Enemy[] enemy;
        Transform[] spawnPoint;
        public int MaxEnemy{get; set;}
    

        Transform[] target;

        void Awake()
        {
            if (parent == null)
            {
                parent = transform.parent;
            }
        }

        void Start()
        {
            enemy = new Enemy.Enemy[enemyPrefabs.Length];
            spawnPoint = new Transform[transform.childCount];
            target = new Transform[spawnPoint.Length];
            for (int i = 0; i < spawnPoint.Length; i++)
            {
                spawnPoint[i] = transform.GetChild(i);
                target[i] = path.FindNearestPoint(spawnPoint[i].position);
            }
            for (int i = 0; i < enemyPrefabs.Length; i++)
            {
                enemy[i] = enemyPrefabs[i].GetComponent<Enemy.Enemy>();
                enemy[i].path = path;
                enemy[i].end = endPoint;

            }
        }
    
        // bug: if invoke too quick, will have out of range exception, i'm not sure why
        public async Task SpawnWave()
        {
            //spawn enemy on preGenerate position, that is out of screen, cache them and spawn them later
            GameObject[] enemys = new GameObject[MaxEnemy];
            int[] spawnIndexArr = new int[MaxEnemy];
            for (int i = 0; i < MaxEnemy; i++)
            {
                int index = Random.Range(0, enemyPrefabs.Length);
                int spawnIndex = Random.Range(0, spawnPoint.Length);
                spawnIndexArr[i] = spawnIndex;
                enemy[index].target = target[spawnIndex];
                enemy[index].spawnPoint = spawnPoint[spawnIndex];
                enemys[i] = Instantiate(enemyPrefabs[index],
                    preGenerate.position, Quaternion.identity);
            }
        
            Tower.Tower.Enemies.AddRange(enemys);
        
            for (int i = 0; i < MaxEnemy; i++)
            {
                Spawn(enemys, spawnIndexArr,i);
                await Task.Delay((int)(waveSpawnTime * 1000 / MaxEnemy));
            }
        }

        void Spawn(GameObject[] enemys, int[] spawnIndexArr , int index)
        {
            if (enemys[index] != null)
            {
                enemys[index].transform.position = spawnPoint[spawnIndexArr[index]].position;
            }

        }

        public Spawner NextLevel()
        {
            int currentIndex = transform.GetSiblingIndex();
            int childCount = parent.childCount;
            if (currentIndex < childCount-1)
            {
                Spawner nextSpawner = parent.GetChild(currentIndex + 1).GetComponent<Spawner>();
                nextSpawner.preGenerate = preGenerate;
                levelEnd?.Invoke();
                return nextSpawner;
            }
            else return this;
        }

        public void OpenSpawnPoint(bool open)
        {
            foreach (Transform point in spawnPoint)
            {
                point.gameObject.SetActive(open);
            }
        }

    }
}
