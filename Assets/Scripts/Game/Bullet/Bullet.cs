using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour,IEndGameObserver
{
    public bool hasReleased = false;
    private Transform _transform;
    /// <summary>
    /// 目标方向
    /// </summary>
    private Vector3 _targetDirection;
    /// <summary>
    /// 子弹速度
    /// </summary>
    private float _bulletSpeed;

    /// <summary>
    /// 子弹伤害
    /// </summary>
    [FormerlySerializedAs("_damage")] public int damage = 10;

    /// <summary>
    /// 子弹的击退距离
    /// </summary>
    private float _knockbackDistance;

    private bool _canHitEnemy;
    
    private void Awake()
    {
        _transform = transform;
        if (SceneManager.GetActiveScene().name=="Level8_Boss3")
        {
            gameObject.layer = LayerMask.NameToLayer("Bullet");
        }
    }

    private void Update()
    {
        MyOnCollision();
        //ShootToTarget();
    }

    private void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
        hasReleased = false;
    }

    private void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        //移除观察者
        GameManager.Instance.RemoveObserver(this);
    }
    

    /// <summary>
    /// 初始化子弹
    /// </summary>
    /// <param name="speed">子弹的速度</param>
    /// <param name="damage">子弹的伤害</param>
    /// <param name="knockbackDistance">子弹的击退距离</param>
    public void InitBullet(float speed,int damage,float knockbackDistance,Vector2 targetDirection)
    {
        _bulletSpeed = speed;
        this.damage = damage;
        _knockbackDistance = knockbackDistance;
        _targetDirection = targetDirection;
    }
    private void ShootToTarget()
    {
        _transform.position += _targetDirection * _bulletSpeed * Time.deltaTime;
        _transform.up = _targetDirection;
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            CharacterStats playerStats = playerController.GetCharacterStats();
            if (playerStats)
            {
                //对玩家造成伤害
                Debug.Log("打到玩家");
                playerStats.TakeDamage(damage);
                //todo 打到玩家或者敌人的爆炸特效
            }
            Vector2 direction = (other.transform.position - transform.position).normalized;
            //玩家击退效果
            playerController.Knockback(direction * _knockbackDistance);
            playerController.SetPlayerStates(PlayerStates.General);
            
            //释放到子弹池
            if (!hasReleased)
            {
                ReleaseBulletToPool();
            }
            
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (_canHitEnemy)
            {
                EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
                CharacterStats enemyStats = enemyController.GetCharacterStats();
                //对敌人造成伤害
                enemyStats.TakeDamage(damage);
                Vector2 direction = (other.transform.position - transform.position).normalized;
                //敌人击退效果
                enemyController.Knockback(direction * _knockbackDistance);
                if (enemyController is Rampager rampager)
                {
                    rampager.StartReturn();
                }
                Debug.Log("反弹到敌人");
            
                //释放到子弹池
                if (!hasReleased)
                {
                    ReleaseBulletToPool();
                }
            }
            
        }
    }
    

    private void OnTriggerExit2D(Collider2D other) 
    {
        //当经过触发器中间线时，将子弹自身的layer改为Bullet
        if (other.gameObject.CompareTag("DividingLine"))
        {
            gameObject.layer = LayerMask.NameToLayer("Bullet");
        }
    }

    /// <summary>
    /// 自己写的碰撞检测 用来实现子弹在墙壁反弹 其实是射线检测 用射线检测来实现 可以优化子弹太快直接穿模的情况 但只是优化 并不能完全解决
    /// </summary>
    private void MyOnCollision()
    {
        
        //获取障碍物层级
        int boundaryLayer = LayerMask.GetMask("Boundary");
        //如果子弹自身的layer为Bullet 才执行射线检测 是为了实现 越过中间线后 子弹在玩家区域进行反弹
        if (gameObject.layer==LayerMask.NameToLayer("Bullet"))
        {
            //射线检测
            RaycastHit2D boundaryHit = Physics2D.Raycast(transform.position, _transform.right, 0.55f, boundaryLayer);
        
            if (boundaryHit)
            {
                //获取接触点法线方向
                Vector3 vect = boundaryHit.normal;
                //获取反射方向
                Vector3 reflect = Vector3.Reflect(_transform.right, vect);
                _transform.right = reflect;
            }
        }
    }

    [Tooltip("需要被攻击的次数")] 
    public int needHitNumber = 2;
    private int _currentHitNumber = 0;
    
    /// <summary>
    /// 子弹被攻击，会改变移动方向，
    /// </summary>
    /// <param name="direction">被攻击后的移动方向</param>
    public void BeAttacked(Vector2 direction)
    {
        _transform.right = direction;
        _currentHitNumber++;
        _transform.GetComponent<SpriteRenderer>().sprite = Respath.BeAttackedBulletSprite;
        if (_currentHitNumber==needHitNumber&&!hasReleased)
        {
            //释放到子弹池
            ReleaseBulletToPool();
        }
    }
    
    /// <summary>
    /// 释放子弹到对象池
    /// </summary>
    public void ReleaseBulletToPool()
    {
        ObjectPool.Recycle(this.gameObject);
        //释放到子弹池的时候 将子弹的layer改为默认 防止无法穿过分界线
        gameObject.layer = LayerMask.NameToLayer("Default");
        //释放到池子时改为不能攻击到敌人
        _canHitEnemy = false;
        //释放到子弹池的时候,将释放状态改为释放了
        hasReleased = true;
        //释放到池子 攻击次数归零
        _currentHitNumber = 0;
        //颜色改为黑色
        _transform.GetComponent<SpriteRenderer>().sprite = Respath.InitialBulletSprite;
    }

    /// <summary>
    /// 获取最近的敌人
    /// </summary>
    /// <returns></returns>
    private Transform GetNearestEnemy()
    {
        // 检测玩家周围的敌人
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 25f);

        // 记录最近的敌人和最小距离
        Collider2D nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        // 迭代每个敌人，计算距离并更新最近的敌人
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(_transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = collider;
                }
            }
        }

        if (nearestEnemy)
        {
            return nearestEnemy.transform;
        }

        return null;


    }

    /// <summary>
    /// 反弹向敌人
    /// </summary>
    public void BounceToEnemy()
    {
        var enemy = GetNearestEnemy();
        if (enemy)
        {
            _canHitEnemy = true;
            _transform.right = (enemy.position - _transform.position).normalized;
            //反弹的时候把子弹的图层改为默认，防止穿不过去分界线
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void GameWinEndNotify()
    {
        ReleaseBulletToPool();
        Debug.Log("子弹释放到对象池");
    }

    public void GameFailEndNotify()
    {
        ReleaseBulletToPool();
        Debug.Log("子弹释放到对象池");
    }
}
