using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveGrid : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;  // Tham chiếu đến Grid Layout Group
    public int minColumns = 3;               // Số lượng cột tối thiểu
    public int maxColumns = 5;               // Số lượng cột tối đa
    public Vector2 cellSize = new Vector2(225f, 100f);  // Kích thước cố định của ô

    void Start()
    {
        AdjustGridColumns();
    }

    void Update()
    {
        AdjustGridColumns();
    }

    void AdjustGridColumns()
    {
        if (gridLayoutGroup == null)
        {
            Debug.LogError("GridLayoutGroup is not assigned.");
            return;
        }

        // Tính toán số lượng cột dựa trên kích thước màn hình và kích thước ô
        int columns = Mathf.Clamp(Screen.width / (int)cellSize.x, minColumns, maxColumns);

        // Cập nhật các giá trị của Grid Layout Group
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;
        gridLayoutGroup.cellSize = cellSize;
    }
}
