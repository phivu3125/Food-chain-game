using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LaneHandler : MonoBehaviour
{
    private TilemapCollider2D tilemapCollider;
    void Start()
    {
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    public void SpawnAnimalAtLane(Animal AnimalPlayer)
    {
        AnimalPlayer.gameObject.tag = "AnimalPlayer";
        Bounds bounds = tilemapCollider.bounds;
        UnityEngine.Vector3 spawnPos = new UnityEngine.Vector3(bounds.min.x, (bounds.min.y + bounds.max.y) / 2, 0);

        Animal spawnedAnimal = AnimalPlayer.spawnAnimal(spawnPos);
        spawnedAnimal.CanRun = true;

        spawnedAnimal.gameObject.name = spawnedAnimal.gameObject.name.Replace("(Clone)", "").Trim();
    }

    public void SpawnEnemy(Animal AnimalEnemy)
    {
        AnimalEnemy.gameObject.tag = "AnimalEnemy";
        Bounds bounds = tilemapCollider.bounds;
        UnityEngine.Vector3 spawnPos = new UnityEngine.Vector3(bounds.max.x, (bounds.min.y + bounds.max.y) / 2, 0);

        Animal spawnedAnimal = AnimalEnemy.spawnAnimal(spawnPos);
        spawnedAnimal.transform.Rotate(0, 180, 0);
        spawnedAnimal.CanRun = true;

        spawnedAnimal.gameObject.name = spawnedAnimal.gameObject.name.Replace("(Clone)", "").Trim();
    }
}
