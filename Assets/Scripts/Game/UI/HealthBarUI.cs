using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    //血条预制体
    public GameObject barHolderPrefab;
    //血条位置
    public Transform healthBarPoint;
    //血条是否长久可见
    public bool alwaysVisible;
    //血条可见时间
    public float visibleTime;
    //血条剩余可见时间
    private float leftTime;
    private CharacterStats currentStats;
    //当前的的HealthBarUI的位置
    private Transform UIBar;
    //滑动条
    private Image slider;
    //相机
    private Transform cam;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpDateHealthBar;
    }

    /// <summary>
    /// 初始化血条
    /// </summary>
    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode==RenderMode.ScreenSpaceCamera)
            {
                UIBar = Instantiate(barHolderPrefab, canvas.transform).transform;
                slider = UIBar.GetChild(0).GetComponent<Image>();
            }
            
        }
    }
    
    /// <summary>
    /// 更新血条 这会儿是敌人和角色共用这个 
    /// </summary>
    /// <param name="currentHealth">当前血量</param>
    /// <param name="MaxHealth">最大血量</param>
    private void UpDateHealthBar(int currentHealth, int MaxHealth)
    {
        if (currentStats.CurrentHealth<=0) Destroy(UIBar.gameObject);
        float sliderPercent = (float)currentHealth / MaxHealth;
        leftTime = visibleTime;
        UIBar.gameObject.SetActive(true);
        slider.fillAmount = sliderPercent;
    }

    private void LateUpdate()
    {
        if (UIBar!=null)
        {
            UIBar.position = transform.position + new Vector3(0, 1.5f, 0);
            UIBar.forward = -cam.forward;
            if (leftTime<=0&&!alwaysVisible)
            {
                UIBar.gameObject.SetActive(false);
            }
            else
            {
                leftTime -= Time.deltaTime;
            }
        }
    }
}
