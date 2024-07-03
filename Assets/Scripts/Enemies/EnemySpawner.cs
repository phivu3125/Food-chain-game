using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyAnimals;
    public float spawnInterval = 5.0f;
    public LaneHandler[] laneHandlers;
    public int initialMaxEnemiesInPhase = 5;
    public int enemiesPerSpawn = 1; // Số lượng quái thả mỗi lần

    private float timeSinceLastSpawn;
    private int currentPhase = 1;
    private int enemiesSpawnedInPhase = 0;
    private int maxEnemiesInPhase;
    private float currentSpawnInterval;
    private bool _isDoneGame = false;

    void Start()
    {        
        maxEnemiesInPhase = initialMaxEnemiesInPhase;
        currentSpawnInterval = spawnInterval;
    }

    void Update()
    {
        if (_isDoneGame) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= currentSpawnInterval)
        {
            if (enemiesSpawnedInPhase < maxEnemiesInPhase)
            {
                SpawnAnimals();
                enemiesSpawnedInPhase += enemiesPerSpawn;
            }
            else
            {
                AdvancePhase();
            }
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnAnimals()
    {
        HashSet<int> usedLanes = new HashSet<int>(); // Đảm bảo không trùng làn
        int actualEnemiesToSpawn = Mathf.Min(enemiesPerSpawn, laneHandlers.Length); // Không thả nhiều hơn số làn có sẵn

        for (int i = 0; i < actualEnemiesToSpawn; i++)
        {
            int attempts = 0;
            int maxAttempts = laneHandlers.Length * 2; // Giới hạn số lần thử
            int randomLane = -1;

            // Tìm làn trống
            while ((randomLane == -1 || usedLanes.Contains(randomLane)) && attempts < maxAttempts)
            {
                randomLane = Random.Range(0, laneHandlers.Length);
                attempts++;
            }

            if (attempts < maxAttempts && randomLane != -1)
            {
                usedLanes.Add(randomLane);
                int randomAnimal = GetRandomAnimalForPhase(currentPhase);
                laneHandlers[randomLane].SpawnEnemy(enemyAnimals[randomAnimal].GetComponent<Animal>());
            }
        }
    }

    int GetRandomAnimalForPhase(int phase)
    {
        // Tăng tỷ lệ xuất hiện của nhện trong các phase sau
        if (phase == 1)
        {
            return Random.Range(0, 2); // Chọn giữa Bird (0) và Spider (1)
        }
        else if (phase == 2)
        {
            return (Random.value > 0.3f) ? 1 : 0; // 70% Spider, 30% Bird
        }
        else
        {
            return (Random.value > 0.5f) ? 1 : 0; // 50% Spider, 50% Bird
        }
    }

    void AdvancePhase()
    {
        currentPhase++;
        enemiesSpawnedInPhase = 0;
        maxEnemiesInPhase += 3; // Tăng số lượng quái tối đa trong mỗi phase để tăng độ khó
        enemiesPerSpawn++; // Tăng số lượng quái thả mỗi lần
        Debug.Log("Advanced to phase " + currentPhase);

        if (currentPhase == 4) 
            _isDoneGame = true;
    }
}
