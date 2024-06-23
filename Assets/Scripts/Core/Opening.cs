using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Opening : MonoBehaviour, IPointerDownHandler
{
    public Button targetButton; // 指向目标按钮
    public float fadeDuration = 4.0f; // 淡入淡出效果的持续时间
    //音效
    
    private static bool hasBeenLoaded = false; // 静态变量用于判断是否首次加载
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // 确保目标按钮有 CanvasGroup 组件
        canvasGroup = targetButton.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = targetButton.gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        if (hasBeenLoaded)
        {
            // 已加载过场景，则隐藏按钮
            targetButton.gameObject.SetActive(false);
        }
        else
        {
            // 第一次加载场景，淡入按钮并标记已加载
            hasBeenLoaded = true;
        }
    }

    // 按下按钮时触发淡出效果并隐藏按钮
    public void OnPointerDown(PointerEventData eventData)
    {
        //播放音效
        StartCoroutine(FadeOutAndDisable());
    }
    
    // 协程实现淡出效果并隐藏按钮
    IEnumerator FadeOutAndDisable()
    {
        float elapsed = 0.0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1.0f - elapsed / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 0.0f; // 完全透明
        targetButton.gameObject.SetActive(false);
    }
}