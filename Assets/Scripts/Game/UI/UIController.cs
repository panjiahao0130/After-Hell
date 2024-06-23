using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VISUALNOVEL;

public class UIController : MonoBehaviour,IEndGameObserver
{
    private Button _nextLevelBtn;
    private Button _rePlayBtn;
    private Button _mainMenuBtn;
    private GameObject _fullScreenPanel;
    public GameObject _guidePanel;
    public LevelType levelType;

    private void Awake()
    {
        //保存当前场景名称
        SaveManager.Instance.SaveSceneName();
        _nextLevelBtn = transform.GetChild(0).GetComponent<Button>();
        _rePlayBtn = transform.GetChild(1).GetComponent<Button>();
        _mainMenuBtn = transform.GetChild(2).GetComponent<Button>();
        _fullScreenPanel = transform.GetChild(3).gameObject;
        _fullScreenPanel.transform.GetComponent<Button>().onClick.AddListener(OnFullScreenClick);
        _nextLevelBtn.onClick.AddListener(OnNextLevelBtnClick);
        _rePlayBtn.onClick.AddListener(OnRePlayBtnClick);
        _mainMenuBtn.onClick.AddListener(OnMainMenuClick);
        _nextLevelBtn.gameObject.SetActive(false);
        _rePlayBtn.gameObject.SetActive(false);
        _mainMenuBtn.gameObject.SetActive(false);
        _fullScreenPanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
        
    }
    
    private void OnDisable()
    {
        if(!GameManager.IsInitialized) return;
        //移除观察者
        GameManager.Instance.RemoveObserver(this);;
    }

    private void OnNextLevelBtnClick()
    {
        if (SaveManager.Instance.NextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            GameManager.Instance.IsGameOver = false;
            SceneManager.LoadSceneAsync(SaveManager.Instance.NextSceneIndex);
        }
    }
    private void OnRePlayBtnClick()
    {
        GameManager.Instance.IsGameOver = false;
        SceneManager.LoadScene(SaveManager.Instance.CurrentScene);
    }
    private void OnMainMenuClick()
    {
        GameManager.Instance.IsGameOver = false;
        SceneManager.LoadSceneAsync("MenuScene");
    }

    private void OnFullScreenClick()
    {

        GameManager.Instance.IsGameOver = false;
        SceneManager.LoadSceneAsync("AVGScene");
    }

    public void OnGuidePanelBtnClick()
    {
        if (_guidePanel)
        {
            _guidePanel.SetActive(false);
            var enemys=FindObjectsOfType<EnemyController>();
            foreach (var enemy in enemys)
            {
                enemy.TransitionState(EnemyStateTypes.Idle);
            }
        }
    }

    public void GameWinEndNotify()
    {
        if (levelType==LevelType.Common)
        {
            _nextLevelBtn.gameObject.SetActive(true);
        }
        else if (levelType==LevelType.Boss)
        {
            _fullScreenPanel.SetActive(true);
        }
        
    }

    public void GameFailEndNotify()
    {
        _rePlayBtn.gameObject.SetActive(true);
        _mainMenuBtn.gameObject.SetActive(true);
    }
}
