using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 先不用，看策划来，要是需要固定位置在屏幕的
/// </summary>
public class PlayerHealthUI : MonoBehaviour
{
    private Image healthSlider;

    /*private Image expSlider;
    private TextMeshProUGUI levelTxt;*/
    private void Awake()
    {
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        /*expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        levelTxt = transform.GetComponentInChildren<TextMeshProUGUI>(true);*/
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateLevelTxt();
        UpDateHealthBar();
        //UpdateExpBar();
    }

    private void UpDateHealthBar()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    /*private void UpdateExpBar()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.characterData.currentExp /
                              GameManager.Instance.playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }*/

    /*rivate void UpdateLevelTxt()
    {
        levelTxt.text="Level "+GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
    }*/
}
