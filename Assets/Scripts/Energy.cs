using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Energy : MonoBehaviour
{
    public float fallSpeed; // Tốc độ rơi của năng lượng
    public float yPosistionToDestroy; // Vị trí Y để hủy năng lượng khi rơi đến
    public int energyValue;  // Giá trị năng lượng sẽ được thêm vào khi thu thập
    public float flySpeed; // Tốc độ bay của năng lượng khi di chuyển về đích

    private Vector3 destroyPos; // Vị trí đích để di chuyển tới
    private GameObject targetText; // Đối tượng văn bản mục tiêu hiển thị số tiền
    private bool canFall = true; // Kiểm tra xem năng lượng có thể rơi hay không

    public bool CanFall { get => canFall; set => canFall = value; }

    void Start()
    {
        InitializeTargetText();
        CalculateDestroyPosition();
    }

    private void InitializeTargetText()
    {
        targetText = GameObject.Find("MoneyText");
        if (targetText == null)
        {
            Debug.LogError("MoneyText object not found in the scene.");
        }
    }

    private void CalculateDestroyPosition()
    {
        if (targetText != null)
        {
            RectTransform rectTransform = targetText.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, rectTransform.position, Camera.main, out destroyPos);
            }
            else
            {
                Debug.LogError("RectTransform component missing on MoneyText object.");
            }
        }
    }

    void Update()
    {
        EnergyFallDown();
        CheckForDestroyEnergy();
    }

    private void EnergyFallDown()
    {
        if (CanFall)
        {
            // Di chuyển năng lượng xuống dưới
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    private void CheckForDestroyEnergy()
    {
        // Hủy năng lượng nếu nó rơi xuống dưới vị trí Y đã định
        if (transform.position.y < yPosistionToDestroy)
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator GainEnergyValue()
    {
        CanFall = false;
        Vector3 direction = gameObject.transform.position - destroyPos;

        // Di chuyển năng lượng về vị trí đích
        while (Vector3.Distance(transform.position, destroyPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destroyPos, flySpeed * Time.deltaTime);
            yield return null;
        }

        // Đảm bảo năng lượng đã di chuyển đến đúng vị trí đích
        transform.position = destroyPos;
        GameManager.Instance.AddMoney(energyValue);
        Destroy(gameObject);
        yield break;
    }


    public Energy SpawnEnergy(Vector3 spawnPos)
    {
        // Tạo ra một đối tượng năng lượng mới tại vị trí spawnPos
        GameObject energyObject = Instantiate(gameObject, spawnPos, Quaternion.identity);
        energyObject.name = energyObject.name.Replace("(Clone)", "").Trim();

        Energy energyComponent = energyObject.GetComponent<Energy>();

        return energyComponent;
    }
}
