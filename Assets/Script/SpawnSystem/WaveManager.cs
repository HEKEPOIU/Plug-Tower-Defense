using System.Threading.Tasks;
using UnityEngine;

namespace SpawnSystem
{
    public class WaveManager : MonoBehaviour
    {
        //Wave monsterNumber is Fibonacci sequence
        public static int CurrentWave { get; set; } = 1;
        int currentLevel = 0;
        [SerializeField] int levelChangeCount = 5;
        Spawner spawners;
        [SerializeField] Transform preGenerate;

        void Start()
        {
            spawners = GetComponentInChildren<Spawner>();
            spawners.preGenerate = preGenerate;

        }
    
        public async Task StartWave()
        {
            CurrentWave++;
            spawners.MaxEnemy = Fibonacci(CurrentWave);
            await spawners.SpawnWave();
            if (CurrentWave % levelChangeCount == 0)
            {
                currentLevel++;
                spawners = spawners.NextLevel();
            }
        }

        int Fibonacci(int num)
        {
            if (num <= 2)
                return 1;
            else
                return Fibonacci(num - 1) + Fibonacci(num - 2);
        }
    }
}
