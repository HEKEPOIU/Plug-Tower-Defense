using System;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveManager : MonoBehaviour
{
    //Wave monsterNumber is Fibonacci sequence
    public static int CurrentWave { get; set; } = 1;
    int currentLevel = 0;
    [SerializeField] int levelChangeCount = 5;
    Spawner[] spawners;

    void Start()
    {
        spawners = new Spawner[transform.childCount];
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).GetComponent<Spawner>();
        }
    }
    
    public void StartWave()
    {
        CurrentWave++;
        spawners[currentLevel].MaxEnemy = Fibonacci(CurrentWave);
        spawners[currentLevel].SpawnWave();
        if (CurrentWave % levelChangeCount == 0 && currentLevel < spawners.Length - 1)
        {
            currentLevel++;
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
