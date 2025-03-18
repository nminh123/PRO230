using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();
}


[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int count;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public List<Wave> waves; 
    public float spawnRadius = 10f;
    public float waveDelay = 5f;

    private int currentWaveIndex = 0;
    private int activeEnemies = 0;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (currentWaveIndex < waves.Count)
        {
            yield return new WaitForSeconds(waveDelay); 
            Debug.Log("Wave " + (currentWaveIndex + 1) + " bắt đầu!");

            foreach (var enemyInfo in waves[currentWaveIndex].enemies)
            {
                for (int i = 0; i < enemyInfo.count; i++)
                {
                    SpawnEnemy(enemyInfo.enemyPrefab);
                }
            }

            currentWaveIndex++;
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 spawnPosition = (Vector2)transform.position + (Random.insideUnitCircle.normalized * spawnRadius);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.OnDeath += OnEnemyDeath;
        }
        activeEnemies++;
    }

    void OnEnemyDeath(int reward)
    {

        if (activeEnemies <= 0 && currentWaveIndex < waves.Count)
        {
            Debug.Log("Tất cả quái đã bị tiêu diệt! Chuẩn bị wave tiếp theo...");
            StartCoroutine(SpawnWave());
        }
    }
}