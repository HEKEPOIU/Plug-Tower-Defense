using System.Threading.Tasks;
using UnityEngine;

namespace SpawnSystem
{
    public class WaveManager : MonoBehaviour
    {
        //Wave monsterNumber is Fibonacci sequence
        Spawner spawners;
        public static int CurrentWave { get; set; } = 1;
        [SerializeField] int levelChangeCount = 5;
        int currentLevel = 0;
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
        
        public void BackLastWave()
        {
            CurrentWave--;
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
