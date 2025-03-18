using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using WorldTime; // Thêm namespace để sử dụng hệ thống thời gian

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
    public worldtime worldTime; // Tham chiếu đến hệ thống thời gian
    public TMP_Text waveNotificationText; // Thông báo trên Canvas
    public int spawnHour = 18; // Giờ cố định để spawn wave
    public int spawnMinute = 0; // Phút cố định để spawn wave

    private int currentWaveIndex = 0;
    private int activeEnemies = 0;
    private bool canSpawn = false;

    void Start()
    {
        worldTime.WorldTimeChanged += OnWorldTimeChanged;
        waveNotificationText.text = "";
    }

    private void OnDestroy()
    {
        worldTime.WorldTimeChanged -= OnWorldTimeChanged;
    }

    private void OnWorldTimeChanged(object sender, TimeSpan currentTime)
    {
        if (currentWaveIndex < waves.Count)
        {
            if (currentTime.Hours == spawnHour && currentTime.Minutes == spawnMinute && !canSpawn)
            {
                canSpawn = true;
                StartCoroutine(SpawnWave());
            }
        }
    }

    IEnumerator SpawnWave()
    {
        if (!canSpawn) yield break;

        waveNotificationText.text = "Wave " + (currentWaveIndex + 1) + " bắt đầu!";
        Debug.Log("Wave " + (currentWaveIndex + 1) + " bắt đầu!");

        foreach (var enemyInfo in waves[currentWaveIndex].enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                SpawnEnemy(enemyInfo.enemyPrefab);
            }
        }

        currentWaveIndex++;
        canSpawn = false;
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 spawnPosition = (Vector2)transform.position + (UnityEngine.Random.insideUnitCircle.normalized * spawnRadius);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.OnDeath += OnEnemyDeath;
        }
        activeEnemies++;
    }

    void OnEnemyDeath()
    {
        activeEnemies--;
        if (activeEnemies <= 0 && currentWaveIndex < waves.Count)
        {
            Debug.Log("Tất cả quái đã bị tiêu diệt! Chuẩn bị wave tiếp theo...");
        }
    }
}
