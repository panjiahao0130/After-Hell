using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawnManager : SingletonMono<EnemySpawnManager>
{
    public int maxEnemies = 10; // 最大敌人数量
    public float spawnMinRadius = 3;
    public float spawnMaxRadius = 11f; // 生成范围半径

    private int currentEnemyNum;
    public bool hasEnemy = false;
    public int CurrentEnemyNum {
        get => currentEnemyNum;
        set
        {
            Debug.Log("当前敌人数量为"+value);
            currentEnemyNum = value;
        }
}

    /// <summary>
    /// 初始化对象池
    /// </summary>
    private void InitObjectPool()
    {
        ObjectPool.CreatePool(Respath.BulletPrefab, 5);
        ObjectPool.CreatePool(Respath.RampagerPrefab, 5);
        ObjectPool.CreatePool(Respath.SharpshooterPrefab, 5);
        ObjectPool.CreatePool(Respath.RiotSoldierPrefab, 5);
        ObjectPool.CreatePool(Respath.FishManSergeantPrefab, 1);
        ObjectPool.CreatePool(Respath.WarMachinePrefab, 1);
        ObjectPool.CreatePool(Respath.GeneralPrefab, 1);
        ObjectPool.CreatePool(Respath.WarMachinePlusPrefab, 1);
        ObjectPool.CreatePool(Respath.SelfDemonPrefab, 1);
        ObjectPool.CreatePool(Respath.BulletTracer, 5);
        ObjectPool.CreatePool(Respath.CannonballPrefab, 5);
        ObjectPool.CreatePool(Respath.WarningLocation, 5);
        ObjectPool.CreatePool(Respath.ExplosionPrefab, 5);
    }

    public override void Awake()
    {
        base.Awake();
        InitObjectPool();
        
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        if (!hasEnemy)
        {
            return;
        }
        if (GameManager.Instance.IsGameOver==false&&CurrentEnemyNum==0)
        {
            GameManager.Instance.IsGameOver = true;
            hasEnemy = false;
            GameManager.Instance.NotifyObservers(GameOverType.Win);
        }
    }

    public GameObject SpawnEnemy(EnemyController enemy, Vector3 position)
    {
        GameObject obj = null;
        switch (enemy.enemyType)
        {
            case EnemyType.Sharpshooter:
                obj= ObjectPool.Spawn(Respath.SharpshooterPrefab,ObjectPool.Instance.transform,position);
                break;
            case EnemyType.Rampager:
                float randomValue = Random.Range(0f, 1f);
                if (randomValue < 0.5f)
                {
                    obj= ObjectPool.Spawn(Respath.RampagerPrefab, ObjectPool.Instance.transform, position);
                }
                else
                {
                    obj= ObjectPool.Spawn(Respath.RiotSoldierPrefab,ObjectPool.Instance.transform,position);
                }
                break;
            
        }
        

        return obj;

    }
    
    public void SpawnEnemy(EnemyType type)
    {
        // 生成随机角度和半径
        float angle = Random.Range(20f, 160f);
        float radius = Random.Range(spawnMinRadius, spawnMaxRadius);

        // 将极坐标转换为笛卡尔坐标
        float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        switch (type)
        {
            case EnemyType.Sharpshooter:
                ObjectPool.Spawn(Respath.SharpshooterPrefab,ObjectPool.Instance.transform,new Vector2(x,y));
                break;
            case EnemyType.Rampager:
                float randomValue = Random.Range(0f, 1f);
                if (randomValue < 0.5f)
                {
                    ObjectPool.Spawn(Respath.RampagerPrefab, ObjectPool.Instance.transform, new Vector2(x, y));
                }
                else
                {
                    ObjectPool.Spawn(Respath.RiotSoldierPrefab,ObjectPool.Instance.transform,new Vector2(x,y));
                }
                break;
            case EnemyType.FishManSergeant:
                ObjectPool.Spawn(Respath.FishManSergeantPrefab,ObjectPool.Instance.transform,new Vector2(x,y));
                break;
            case EnemyType.WarMachine:
                ObjectPool.Spawn(Respath.WarMachinePrefab,ObjectPool.Instance.transform,new Vector2(x,y));
                break;
            case EnemyType.General:
                ObjectPool.Spawn(Respath.GeneralPrefab,ObjectPool.Instance.transform,new Vector2(x,y));
                break;
            case EnemyType.WarMachinePlus:
                ObjectPool.Spawn(Respath.WarMachinePrefab,ObjectPool.Instance.transform,new Vector2(x,y));
                break;
            case EnemyType.SelfDemon:
                ObjectPool.Spawn(Respath.SelfDemonPrefab,ObjectPool.Instance.transform,new Vector2(x,y));
                break;
        }
        CurrentEnemyNum++;
    }
    

    /*private int CountEnemies()
    {
        // 计算场景中敌人的数量
        Enemy[] enemies = FindObjectsOfType(typeof(Enemy)) as Enemy[];
        return enemies.Length;
    }*/
}