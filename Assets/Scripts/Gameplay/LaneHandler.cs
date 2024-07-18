using System;
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
    private int _laneNumber;

    private void Start()
    {
        char lastCharInName = gameObject.name[gameObject.name.Length - 1];
        _laneNumber = Convert.ToInt32(lastCharInName);

        string _firstCellName = $"Cell{lastCharInName}1";
        _firstCell = transform.Find(_firstCellName);

        string _lastCellName = $"Cell{lastCharInName}9";
        _lastCell = transform.Find(_lastCellName);
    }
    public void SpawnAnimalAtLane(Animal AnimalPlayer)
    {
        if (AnimalPlayer.name.Equals("Flower")) return; // Xử lí spawn Flower ở chỗ khác

        AnimalPlayer.gameObject.tag = "AnimalPlayer";
        AnimalPlayer.transform.rotation  = UnityEngine.Quaternion.Euler(30, 0, 0);

        Animal spawnedAnimal = AnimalPlayer.SpawnAnimal(_firstCell.position);
        spawnedAnimal.CanRun = true;
    }

    static public LaneHandler GetLaneByName(String name)
    {
        return GameObject.Find(name).GetComponent<LaneHandler>();
    }


    public void SpawnEnemy(Animal AnimalEnemy)
    {
        AnimalEnemy.gameObject.tag = "AnimalEnemy";

        Animal spawnedAnimal = AnimalEnemy.SpawnAnimal(_lastCell.position);
        spawnedAnimal.transform.localRotation  = UnityEngine.Quaternion.Euler(-30, 180, 0);
        spawnedAnimal.CanRun = true;
    }
}
