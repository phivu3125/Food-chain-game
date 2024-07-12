using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    public void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        // Chuyển đổi các giá trị safe area thành tỉ lệ phần trăm của kích thước màn hình
        Vector2 anchorMin = new Vector2(safeArea.x / Screen.width, safeArea.y / Screen.height);
        Vector2 anchorMax = new Vector2((safeArea.x + safeArea.width) / Screen.width, (safeArea.y + safeArea.height) / Screen.height);

        // Thiết lập anchors cho RectTransform
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        // Đặt lại offset để đảm bảo không có dịch chuyển bất thường
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
public class SafeAreaManager : MonoBehaviour
{
    public List<RectTransform> panels;  // Danh sách các panel cần áp dụng SafeArea

    void Start()
    {
        ApplySafeAreaToSpecifiedPanels();
    }

    void ApplySafeAreaToSpecifiedPanels()
    {
        foreach (RectTransform panel in panels)
        {
            if (panel != null && IsPanelVisible(panel))
            {
                SafeArea safeArea = panel.gameObject.AddComponent<SafeArea>();
                safeArea.ApplySafeArea();  // Áp dụng SafeArea ngay lập tức
            }
            else
            {
                Debug.LogWarning("Panel is null. Please check your SafeAreaManager panel list.");
            }
        }
    }

    bool IsPanelVisible(RectTransform panel)
    {
        return panel.gameObject.activeSelf;
    }
}
