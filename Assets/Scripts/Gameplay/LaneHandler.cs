using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LaneHandler : MonoBehaviour
{
    private Transform _firstCell;
    private Transform _lastCell;

    private void Start() {
        char lastCharInName = gameObject.name[gameObject.name.Length - 1];
        string  _firstCellName = $"Cell{lastCharInName}1";
        _firstCell = transform.Find(_firstCellName);

        string  _lastCellName = $"Cell{lastCharInName}10";
        _lastCell = transform.Find(_lastCellName);
    }
    public void SpawnAnimalAtLane(Animal AnimalPlayer)
    {
        AnimalPlayer.gameObject.tag = "AnimalPlayer";

        Animal spawnedAnimal = AnimalPlayer.spawnAnimal(_firstCell.position);
        spawnedAnimal.CanRun = true;

        spawnedAnimal.gameObject.name = spawnedAnimal.gameObject.name.Replace("(Clone)", "").Trim();
    }

    public void SpawnEnemy(Animal AnimalEnemy)
    {
        AnimalEnemy.gameObject.tag = "AnimalEnemy";
        
        Animal spawnedAnimal = AnimalEnemy.spawnAnimal(_lastCell.position);
        spawnedAnimal.transform.Rotate(0, 180, 0);
        spawnedAnimal.CanRun = true;

        spawnedAnimal.gameObject.name = spawnedAnimal.gameObject.name.Replace("(Clone)", "").Trim();
    }
}
