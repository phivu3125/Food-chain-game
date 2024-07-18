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
    public GameObject Mask;

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
        Mask.transform.position = transform.position + Vector3.up;
        Mask.SetActive(true);
        
        // Nếu không phải là hoa (flower), tạo một instance của arrowAnimated
        if (!isFlower)
        {
            // Tạo một đối tượng arrowAnimated tại vị trí tương ứng với cell hiện tại
            Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, transform.position.y + 1f, transform.position.z);
            arrowAnimtedInstance = Instantiate(arrowAnimated, spawnPosition, Quaternion.Euler(90, 0, 0));
        }
    }

    public void UnHighlightCell(bool isFlower)
    {
        Mask.SetActive(false);
        
        if (!isFlower)
        {
            Destroy(arrowAnimtedInstance);
        }
    }
}
