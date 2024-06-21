using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public InputReader inputReader;
    public GameObject _overlay;
    private bool _isDragging = false;
    private Animal _draggedAnimal;
    private AnmialCard _selectedAnimalCard;
    private bool _isSelected = false;

    void Start()
    {
        inputReader.OnPointerClicked += OnDragStart;
        inputReader.OnPointerClickedRelease += OnDragEnd;
        inputReader.OnPointerDrag += OnDrag;
        _overlay = Instantiate(_overlay, new Vector3(0, 0, 0), Quaternion.identity);
        _overlay.SetActive(false);
    }

    private void OnDestroy()
    {
        inputReader.OnPointerClicked -= OnDragStart;
        inputReader.OnPointerClickedRelease -= OnDragEnd;
        inputReader.OnPointerDrag -= OnDrag;
    }

    private void OnDragStart()
    {
        _isDragging = true;
    }

    private void OnDrag(Vector2 dragPos)
    {
        if (_isDragging)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(dragPos.x, dragPos.y, Camera.main.nearClipPlane));
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null && hit.transform.CompareTag("Card"))
            {
                _selectedAnimalCard = hit.transform.GetComponent<AnmialCard>();
                Animal animal = _selectedAnimalCard.animal;
                if (_selectedAnimalCard != null && !_isSelected)
                {
                    _draggedAnimal = animal.spawnAnimal(worldPosition);
                }
                _isSelected = true;
            }

            if (_draggedAnimal != null && _isSelected)
            {
                _overlay.SetActive(true);
                _draggedAnimal.transform.position = new Vector3(worldPosition.x, worldPosition.y, _draggedAnimal.transform.position.z);
            }
        }

    }


    private void OnDragEnd()
    {
        _isDragging = false;
        if (_draggedAnimal != null)
        {
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(_draggedAnimal.transform.position);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Lane"))
                {
                    LaneHandler laneHandler = hitCollider.GetComponent<LaneHandler>();
                    if (laneHandler != null)
                    {
                        laneHandler.SpawnAnimalAtLane(_draggedAnimal.GetComponent<Animal>());
                    }
                }
            }
            Destroy(_draggedAnimal.gameObject);
            _draggedAnimal = null;
        }
        _selectedAnimalCard = null;
        _isSelected = false;
        _overlay.SetActive(false);
    }
}
