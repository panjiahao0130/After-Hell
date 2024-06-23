using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected bool isRemove = false;
    protected new string name;
    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OpenPanel(string name)
    {
        this.name = name;
        SetActive(true);
        /*
         //淡入效果
        CanvasGroup canvasGroup=GetComponent<CanvasGroup>();
        canvasGroup.alpha=0.0f;
        DOTween.To(()=>canvasGroup.alpha,x =>canvasGroup.alpha= x,1f, 1);*/
    }

    public virtual void ClosePanel()
    {
        isRemove = true;
        SetActive(false);
        Destroy(gameObject);
        if(UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
        /*
        //淡出效果 放子类中
        CanvasGroup canvasGroup=GetComponent<CanvasGroup>();
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, 1).OnComplete(() =>
        {
            base.ClosePanel();
        });*/
    }
}
