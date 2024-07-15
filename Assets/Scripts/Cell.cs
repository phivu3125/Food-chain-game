using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Cell : MonoBehaviour
{
    private Animal flowerInCell = null;
    [SerializeField] private GameObject arrowAnimated;
    private GameObject arrowAnimtedInstance;
    public float xOffset;


    public Animal FlowerInCell { get => flowerInCell; set => flowerInCell = value; }

    public void SpawnFlowerAtCell(Animal flower)
    {
        flower.gameObject.tag = "AnimalPlayer";

        Animal spawnedFlower = flower.SpawnAnimal(transform.position);
        spawnedFlower.CanRun = false;
        FlowerInCell = spawnedFlower;
    }

    public void HighlightCell(bool isFlower)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = -1;
        }

        // Nếu không phải là hoa (flower), tạo một instance của arrowAnimated
        if (!isFlower)
        {
            // Tạo một đối tượng arrowAnimated tại vị trí tương ứng với cell hiện tại
            Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
            arrowAnimtedInstance = Instantiate(arrowAnimated, spawnPosition, Quaternion.identity);
        }
    }

    public void UnHighlightCell(bool isFlower)
    {
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 1;
        }
        if (!isFlower)
        {
            Destroy(arrowAnimtedInstance);
        }
    }
}
