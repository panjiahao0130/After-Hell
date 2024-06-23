using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ending : MonoBehaviour
{
    public float scrollDuration = 10f; // 滚动持续时间
    public Button targetButton; // 目标Button对象
    public float fadeDuration = 1f; // Button渐入时间

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // 获取初始位置和目标位置
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = new Vector2(startPosition.x, startPosition.y + Screen.height + rectTransform.rect.height);

        // 使用DOTween进行平滑滚动
        rectTransform.DOAnchorPos(endPosition, scrollDuration).SetEase(Ease.Linear).OnComplete(OnScrollComplete);

       
    }

    void OnScrollComplete()
    {
        
        // 动画完成后显示Button并进行渐入
        targetButton.gameObject.SetActive(true);
        CanvasGroup buttonCanvasGroup = targetButton.GetComponent<CanvasGroup>();
        buttonCanvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad);

        // 销毁当前滚动文本的游戏对象
        gameObject.SetActive(false);
    }
}