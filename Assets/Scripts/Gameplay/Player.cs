using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public InputReader inputReader;
    public GameObject overlayPrefab;
    private bool isDragging = false;
    private Animal draggedAnimal;
    private AnimalCard selectedAnimalCard;
    private bool isSelected = false;
    private GameObject overlayInstance;

    void Start()
    {
        RegisterInputEvents();
        InitializeOverlay();
    }

    private void OnDestroy()
    {
        UnregisterInputEvents();
    }

    private void RegisterInputEvents()
    {
        inputReader.OnPointerClicked += OnDragStart;
        inputReader.OnPointerClickedRelease += OnDragEnd;
        inputReader.OnPointerDrag += OnDrag;
    }

    private void UnregisterInputEvents()
    {
        inputReader.OnPointerClicked -= OnDragStart;
        inputReader.OnPointerClickedRelease -= OnDragEnd;
        inputReader.OnPointerDrag -= OnDrag;
    }

    private void InitializeOverlay()
    {
        overlayInstance = Instantiate(overlayPrefab, Vector3.zero, Quaternion.identity);
        overlayInstance.SetActive(false);
   
    }

    private void OnDragStart()
    {
        isDragging = true;
    }

    private void OnDrag(Vector2 dragPos)
    {
        if (isDragging)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(dragPos.x, dragPos.y, Camera.main.nearClipPlane));
            HandleDragging(worldPosition);
        }
    }

    private void HandleDragging(Vector3 worldPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null && hit.transform.CompareTag("Card") && !isSelected)
        {
            AnimalCard animalCard = hit.transform.GetComponent<AnimalCard>();
            if (!animalCard.IsEnoughMoney && !animalCard.IsLoading)
            {
                SelectAnimalCard(hit.transform.GetComponent<AnimalCard>(), worldPosition);
            }
        }

        if (draggedAnimal != null && isSelected)
        {
            overlayInstance.SetActive(true);
            draggedAnimal.transform.position = new Vector3(worldPosition.x, worldPosition.y, draggedAnimal.transform.position.z);
        }
    }

    private void SelectAnimalCard(AnimalCard animalCard, Vector3 spawnPosition)
    {
        selectedAnimalCard = animalCard;
        Animal animal = selectedAnimalCard.animal;
        if (selectedAnimalCard != null && !isSelected)
        {
            draggedAnimal = animal.spawnAnimal(spawnPosition);
        }
        isSelected = true;
    }

    private void OnDragEnd()
    {
        isDragging = false;
        if (draggedAnimal != null)
        {
            TryPlaceAnimal();
            Destroy(draggedAnimal.gameObject);
            ResetDraggingState();
        }
    }

    private void TryPlaceAnimal()
    {
        if (CanPlaceAnimal(out LaneHandler laneHandler))
        {
            int animalCost = selectedAnimalCard.CostToDrop;

            if (GameManager.Instance.SpendMoney(animalCost))
            {
                laneHandler.SpawnAnimalAtLane(draggedAnimal.GetComponent<Animal>());
                selectedAnimalCard.StartSpawnTimer();
            }
            else
            {
                Debug.Log("Not enough money to drop the animal.");
            }
        }
        else
        {
            Debug.Log("Invalid place to drop the animal.");
        }
    }

    private bool CanPlaceAnimal(out LaneHandler laneHandler)
    {
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(draggedAnimal.transform.position);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Lane"))
            {
                laneHandler = hitCollider.GetComponent<LaneHandler>();
                if (laneHandler != null)
                {
                    return true;
                }
            }
        }
        laneHandler = null;
        return false;
    }

    private void ResetDraggingState()
    {
        draggedAnimal = null;
        selectedAnimalCard = null;
        isSelected = false;
        overlayInstance.SetActive(false);
    }
}
