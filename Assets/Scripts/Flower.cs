using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Animal
{
    public float energySpawnInterval; // Khoảng thời gian giữa các lần spawn năng lượng
    public Energy energyPrefab; // Prefab năng lượng
    private float _timeSinceLastSpawnEnergy = 0;
    private Energy energyInstance = null;

    private new void Update()
    {
        SpawnEnergy();
    }

    private void SpawnEnergy()
    {
        _timeSinceLastSpawnEnergy += Time.deltaTime;
        if (_timeSinceLastSpawnEnergy > energySpawnInterval && energyInstance == null)
        {
            energyInstance = energyPrefab.SpawnEnergy(transform.position);
            energyInstance.CanFall = false;
            _timeSinceLastSpawnEnergy = 0;
        }
    }
}
