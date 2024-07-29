using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    private float _timeSinceLastSpawnEnergy = 0;
    public Energy energy;
    public float timeSpawn;
    [SerializeField] float[] xBounds = new float[2];
    [SerializeField] float[] zBounds = new float[2];
    [SerializeField] float fixedY;

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
        float randomX = Random.Range(xBounds[0], xBounds[1]);
        float randomZ = Random.Range(zBounds[0], zBounds[1]);

        _timeSinceLastSpawnEnergy += Time.deltaTime;
        if (_timeSinceLastSpawnEnergy > timeSpawn)
        {
            Vector3 spawnPos = new Vector3(randomX, fixedY, randomZ);

            energy.SpawnEnergy(spawnPos);
            _timeSinceLastSpawnEnergy = 0;
        }
    }
}
