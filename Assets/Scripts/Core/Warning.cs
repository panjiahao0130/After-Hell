using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Warning : MonoBehaviour
{
    public float fadeInDuration = 2f; // 淡入时间
    public float displayDuration = 5f; // 停留时间
    public float fadeOutDuration = 2f; // 淡出时间
    public CanvasGroup uicg;
    public CanvasGroup canvasGroup;

    void Awake()
    {
        // 获取或添加CanvasGroup组件


        // 设置CanvasGroup的alpha为0
        uicg.alpha = 0.0001f;
        canvasGroup.alpha = 0f;
    }

    void OnEnable()
    {
        // 执行Warning对象的淡入、停留和淡出效果
        canvasGroup.DOFade(1f, fadeInDuration)
            .OnComplete(() => 
                DOVirtual.DelayedCall(displayDuration, () => 
                    canvasGroup.DOFade(0f, fadeOutDuration)
                        .OnComplete(() => 
                        {
                            gameObject.SetActive(false);
                            // 另一个游戏对象的淡入效果
                            uicg.DOFade(1f, fadeInDuration).SetEase(Ease.InOutQuad);
                        })
                )
            );
    }
}