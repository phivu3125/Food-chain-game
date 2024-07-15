using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    private float _timeSinceLastSpawnEnergy = 0;
    public Energy energy;
    public float timeSpawn;
    private void Start()
    {
        _timeSinceLastSpawnEnergy = 10;
    }

    private void Update()
    {
        SpawnEnergy();
    }

    public void SpawnEnergy()
    {
        float randomX = Random.Range(-4f, 6f);
        float fixedY = 6f;

        _timeSinceLastSpawnEnergy += Time.deltaTime;
        if (_timeSinceLastSpawnEnergy > timeSpawn)
        {
            Vector3 spawnPos = new Vector3(randomX, fixedY, 0);

            energy.SpawnEnergy(spawnPos);
            _timeSinceLastSpawnEnergy = 0;
        }
    }
}
