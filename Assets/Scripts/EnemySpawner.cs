using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyAnimals; 
    public float spawnInterval = 5.0f;
    public LaneHandler[] laneHandlers;

    private float timeSinceLastSpawn;

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnAnimal();
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnAnimal()
    {
        int randomLane = Random.Range(0, laneHandlers.Length);
        int randomAnimal = Random.Range(0, enemyAnimals.Length);
        laneHandlers[randomLane].SpawnEnemy(enemyAnimals[randomAnimal].GetComponent<Animal>());
    }
}
