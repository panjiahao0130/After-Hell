using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEndGameObserver
{
    void GameWinEndNotify()
    {
        Debug.Log("游戏胜利通知");
    }

    void GameFailEndNotify()
    {
        Debug.Log("游戏失败通知");
    }
}
