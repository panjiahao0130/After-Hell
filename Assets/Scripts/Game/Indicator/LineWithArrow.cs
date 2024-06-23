using UnityEngine;

public class LineWithArrow : MonoBehaviour
{
    public Transform lineStart; // 线段的起点
    public SpriteRenderer arrowRenderer; // 箭头的渲染器

    private LineRenderer lineRenderer;

    void Start()
    {
        // 获取线段的LineRenderer组件
        lineRenderer = GetComponentInChildren<LineRenderer>(true);
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found!");
            return;
        }

        
    }

    void Update()
    {
        // 设置线段的起点
        lineRenderer.SetPosition(0, lineStart.position);
        // 获取鼠标位置
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // 更新线段的终点为鼠标位置
        lineRenderer.SetPosition(1, mousePosition);

        // 计算箭头朝向
        Vector3 arrowDirection = (mousePosition - lineStart.position).normalized;

        // 设置箭头的位置和朝向
        arrowRenderer.transform.position = mousePosition;
        arrowRenderer.transform.right = arrowDirection;
    }
}