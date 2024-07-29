using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InputReader inputReader;

    private bool isDragging = false;
    private Animal draggedAnimal;
    private AnimalCard selectedAnimalCard;
    private bool isSelected = false;
    private Cell currentHighlightedCell = null;
    void Awake()
    {
        GameManager.Instance.ResetGameManager();
    }
    void Start()
    {
        RegisterInputEvents(); // Đăng ký các sự kiện đầu vào
    }

    private void OnDestroy()
    {
        UnregisterInputEvents(); // Hủy đăng ký các sự kiện đầu vào khi đối tượng bị hủy
    }

    private void RegisterInputEvents()
    {
        // Đăng ký các sự kiện đầu vào
        inputReader.OnPointerClicked += OnDragStart;
        inputReader.OnPointerClickedRelease += OnDragEnd;
        inputReader.OnPointerDrag += OnDrag;
        inputReader.OnPointerClicked += OnClicked;
    }

    private void UnregisterInputEvents()
    {
        // Hủy đăng ký các sự kiện đầu vào
        inputReader.OnPointerClicked -= OnDragStart;
        inputReader.OnPointerClickedRelease -= OnDragEnd;
        inputReader.OnPointerDrag -= OnDrag;
        inputReader.OnPointerClicked -= OnClicked;

    }

    private void OnClicked()
    {
        // Xử lý khi click chuột để nhặt năng lượng
        Vector3 worldPosition = GetWorldPositionFromPointer(inputReader.PointerPos);
        HandlePickEnergy(worldPosition);
    }

    private Vector3 GetWorldPositionFromPointer(Vector2 pointerPos)
    {
        // Chuyển đổi vị trí con trỏ từ không gian màn hình sang không gian thế giới
        return Camera.main.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, Camera.main.nearClipPlane));
    }

    private void HandlePickEnergy(Vector3 worldPosition)
    {
        // Kiểm tra va chạm với các đối tượng năng lượng và thêm năng lượng
        Vector2 PointerPos = Camera.main.WorldToScreenPoint(worldPosition); // Lấy vị trí trên màn hình vì thẻ nằm theo UI;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(PointerPos.x, PointerPos.y, 0f));
        RaycastHit[] hits3D = Physics.RaycastAll(ray);

        foreach (RaycastHit hit3D in hits3D)
        {
            if (hit3D.collider != null && hit3D.collider.CompareTag("Energy"))
            {
                Energy energy = hit3D.transform.GetComponent<Energy>();
                StartCoroutine(energy.GainEnergyValue());
            }
        }
    }

    private void OnDragStart()
    {
        isDragging = true; // Bắt đầu kéo
    }

    private void OnDrag(Vector2 dragPos)
    {
        if (isDragging)
        {
            // Xử lý kéo thả động vật
            Vector3 worldPosition = GetWorldPositionFromPointer(dragPos);
            HandleDragging(worldPosition);
        }
    }

    private void HandleDragging(Vector3 worldPosition)
    {
        // Xử lý kéo thả động vật
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition); // Lấy vị trí trên màn hình vì thẻ nằm theo UI
        RaycastHit2D hit = Physics2D.Raycast(screenPosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("UI")); // Card sẽ được đánh là layer UI
        if (hit.collider != null && hit.transform.CompareTag("Card") && !isSelected)
        {
            AnimalCard animalCard = hit.transform.GetComponent<AnimalCard>();
            if (!animalCard.IsEnoughMoney && !animalCard.IsLoading)
            {
                SelectAnimalCard(animalCard, screenPosition);
            }
        }

        if (draggedAnimal != null && isSelected)
        {

            HandleCellHighlighting(worldPosition); // Xử lý làm nổi bật ô

            Vector2 PointerPos = screenPosition;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(PointerPos.x, PointerPos.y, 0f));
            RaycastHit[] hits3D = Physics.RaycastAll(ray);

            bool laneHit = false;
            foreach (RaycastHit hit3D in hits3D)
            {
                if (hit3D.collider != null && hit3D.collider.CompareTag("Lane"))
                {
                    draggedAnimal.transform.position = hit3D.point;
                    laneHit = true;
                    break;
                }
            }

            if (!laneHit)
            {
                draggedAnimal.transform.position = new Vector3(worldPosition.x, worldPosition.y, 100f);
            }
        }
    }

    private void HandleCellHighlighting(Vector3 worldPosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(draggedAnimal.transform.position, 5f);
        bool cellHit = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Cell"))
            {
                Cell cell = hitCollider.GetComponent<Cell>();
                bool isFlower = draggedAnimal.name.Equals("Flower");

                if (isFlower && cell.FlowerInCell == null || !isFlower)
                {
                    UpdateHighlightedCell(cell, isFlower);
                    cellHit = true;
                    break;
                }
            }
        }

        if (!cellHit && currentHighlightedCell != null)
        {
            UnHighlightCurrentCell(); // Bỏ làm nổi bật ô hiện tại
        }
    }

    private void UpdateHighlightedCell(Cell cell, bool isFlower)
    {
        // Làm nổi bật hoặc bỏ làm nổi bật ô
        if (cell != currentHighlightedCell)
        {
            if (currentHighlightedCell != null)
            {
                currentHighlightedCell.UnHighlightCell(isFlower);
            }
            cell.HighlightCell(isFlower);
            currentHighlightedCell = cell;
        }
    }

    private void UnHighlightCurrentCell()
    {
        // Bỏ làm nổi bật ô hiện tại
        if (currentHighlightedCell != null)
        {
            currentHighlightedCell.UnHighlightCell(draggedAnimal.name.Equals("Flower"));
            currentHighlightedCell = null;
        }
    }

    private void SelectAnimalCard(AnimalCard animalCard, Vector2 screenPosition)
    {
        Vector3 spawnPosition = GetWorldPositionFromPointer(screenPosition);
        // Chọn thẻ động vật để kéo thả
        selectedAnimalCard = animalCard;
        Animal animal = selectedAnimalCard.animal;

        if (selectedAnimalCard != null && !isSelected)
        {
            draggedAnimal = animal.SpawnAnimal(spawnPosition);
        }
        isSelected = true;
    }

    private void OnDragEnd()
    {
        isDragging = false; // Kết thúc kéo
        if (draggedAnimal != null)
        {
            TryPlaceAnimalOrFlower(); // Thử đặt động vật hoặc hoa

            UnHighlightCurrentCell(); // Bỏ làm nổi bật ô hiện tại
            Destroy(draggedAnimal.gameObject); // Hủy đối tượng draggedAnimal sau khi thả
            ResetDraggingState(); // Đặt lại trạng thái kéo thả
        }
    }

    private void TryPlaceAnimalOrFlower()
    {
        // Thử đặt động vật hoặc hoa
        if (draggedAnimal.name.Equals("Flower"))
        {
            if (CanPlaceFlower(out Cell cell))
            {
                PlaceAnimalOrFlower(cell);
            }
            else
            {
                Debug.Log("Invalid place to drop the flower.");
            }
        }
        else
        {
            if (CanPlaceAnimal(out LaneHandler laneHandler))
            {
                PlaceAnimalOrFlower(laneHandler);
            }
            else
            {
                Debug.Log("Invalid place to drop the animal.");
            }
        }
    }

    private void PlaceAnimalOrFlower<T>(T handler)
    {
        int animalCost = selectedAnimalCard.CostToDrop;
        if (GameManager.Instance.SpendMoney(animalCost))
        {
            if (handler is LaneHandler laneHandler)
            {
                laneHandler.SpawnAnimalAtLane(draggedAnimal.GetComponent<Animal>());
            }
            else if (handler is Cell cell)
            {
                cell.SpawnFlowerAtCell(draggedAnimal.GetComponent<Animal>());
            }
            selectedAnimalCard.StartSpawnTimer();
        }
        else
        {
            Debug.Log("Not enough money to drop the animal.");
        }
    }

    private bool CanPlaceAnimal(out LaneHandler laneHandler)
    {
        Collider[] hitColliders = Physics.OverlapSphere(draggedAnimal.transform.position, 5f);

        bool hasCell = false;
        laneHandler = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Cell"))
            {
                hasCell = true;
            }
            else if (hitCollider.CompareTag("Lane"))
            {
                laneHandler = hitCollider.GetComponent<LaneHandler>();
            }
        }

        return hasCell && laneHandler != null;
    }

    private bool CanPlaceFlower(out Cell cell)
    {
        // Kiểm tra xem có thể đặt hoa vào ô không
        Collider[] hitColliders = Physics.OverlapSphere(draggedAnimal.transform.position, 5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Cell"))
            {
                cell = hitCollider.GetComponent<Cell>();
                if (cell != null && cell.FlowerInCell == null)
                {
                    return true;
                }
            }
        }
        cell = null;
        return false;
    }

    private void ResetDraggingState()
    {
        // Đặt lại trạng thái kéo thả
        draggedAnimal = null;
        selectedAnimalCard = null;
        isSelected = false;
    }
}

