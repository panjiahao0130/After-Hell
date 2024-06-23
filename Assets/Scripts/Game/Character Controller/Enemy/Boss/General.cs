using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class General : EnemyController
{
    public Transform[] spawnPoints; // 敌人生成点
    public int totalEnemies = 3; // 总敌人数
    private int currentEnemyCount = 3; // 当前敌人数
    public float spawnInterval = 3;
    public List<EnemyController> needSpawnEnemys = new List<EnemyController>();
    
    protected override void InitializeStatesDictionary()
    {
        base.InitializeStatesDictionary();
        states.Add(EnemyStateTypes.ThrowSoldiers, new ThrowSoldiersState(this));
    }
    
    // 生成敌人
    public void SpawnEnemies(int index)
    {
        var enemy=EnemySpawnManager.Instance.SpawnEnemy(needSpawnEnemys[index],transform.position);
        enemy.GetComponent<EnemyController>().TransitionState(EnemyStateTypes.Stop);
        enemy.AddComponent<BeThrowedEnemy>().SetBoss(this);
    }

    // 当敌人被击杀时调用此方法
    public void OnEnemyKilled()
    {
        currentEnemyCount--;

        if (currentEnemyCount <= 0)
        {
            // Boss死亡
            ObjectPool.Recycle(gameObject);
        }
    }
    
}
