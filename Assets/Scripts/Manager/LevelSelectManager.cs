using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager :MonoBehaviour
{
    public GameObject levelSelectPanel;
    private Button[] levelSelectBtns;
    private int unLockedLevelIndex;

    private void Start()
    {
        levelSelectBtns = new Button[levelSelectPanel.transform.childCount];
        for (int i = 0; i < levelSelectPanel.transform.childCount; i++)
        {
            int index = i+1;
            levelSelectBtns[i] = levelSelectPanel.transform.GetChild(i).GetComponent<Button>();
            levelSelectBtns[i].onClick.AddListener(()=>OnLevelButtonClick(index));
        }
        //先全部设为不可点击状态
        foreach (var t in levelSelectBtns)
        {
            t.interactable = false;
        }

        //再根据当前的关卡的解锁情况使已经解锁的关卡可以点击
        for (var index = 0; index < unLockedLevelIndex+1; index++)
        {
            levelSelectBtns[index].interactable = true;
        }
    }

    private void OnLevelButtonClick(int index)
    {
        SceneManager.LoadScene("Level" + index);
    }
}
