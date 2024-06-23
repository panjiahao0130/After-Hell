using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    //观察者列表
    private List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    public bool isGameOver = false;

    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            // 当参数改变时执行的方法
            Debug.Log("MyParameter has changed to: " + value);
            isGameOver = value;
        }
    }

    
    [HideInInspector]
    public CharacterStats playerStats;
    
    //private bool isGameOver = false;

    /// <summary>
    /// 注册角色数据
    /// </summary>
    /// <param name="player">player的数据</param>
    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;
    }
    
    /// <summary>
    /// 添加观察者
    /// </summary>
    /// <param name="observer"></param>
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    /// <summary>
    /// 移除观察者
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }
    
    
    /// <summary>
    /// 通知所有观察者
    /// </summary>
    public void NotifyObservers(GameOverType overType)
    {
        if (IsGameOver)
        {
            if (overType==GameOverType.Win)
            {
                for (int i = 0; i < endGameObservers.Count; i++)
                {
                    endGameObservers[i].GameWinEndNotify();
                }
            }
            else if (overType==GameOverType.Fail)
            {
                for (int i = 0; i < endGameObservers.Count; i++)
                {
                    endGameObservers[i].GameFailEndNotify();
                }
            }
        }
    }

}
