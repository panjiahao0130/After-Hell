using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarMachine : EnemyController
{
    [Tooltip("撞击速度")]
    public float moveSpeed = 5f; // 移动速度

    [HideInInspector]
    public bool HasDash;
    private GameObject _cannonBallObj; // 炮弹预制体
    public float preAttackTime = 1f; // 前摇时间
    public int numberOfBullets = 3; // 发射的炮弹数量
    public GameObject fireObj;
    private Vector2 targetPosition;
    public float interval = 8;

    protected override void InitializeStatesDictionary()
    {
        base.InitializeStatesDictionary();
        states.Add(EnemyStateTypes.Dash, new DashState(this));
        states.Add(EnemyStateTypes.LaunchCannon,new LaunchCannonState(this));
    }
    //击退距离
    [Tooltip("敌人撞击玩家时的击退距离")]
    public float knockbackDistance = 20f;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerController = other.gameObject.GetComponent<PlayerController>();
            CharacterStats playerStats = _playerController.GetCharacterStats();
            if (_characterStats)
            {
                playerStats.TakeDamage(_characterStats);
            }
            Vector2 direction = (other.transform.position - transform.position).normalized;
            _playerController.Knockback(direction * knockbackDistance);
            //玩家被撞到之后状态改为一般状态
            _playerController.SetPlayerStates(PlayerStates.General);
            
            //撞到玩家后自己的反弹
            Knockback(-direction*knockbackDistance);
            //撞到玩家状态改为发射火炮状态
            StartCoroutine(DelayTransitionState());

        }

        if (other.gameObject.CompareTag("Boundary"))
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            //撞到玩家后自己的反弹
            Knockback(-direction*knockbackDistance);
            StartCoroutine(DelayTransitionState());
        }
    }

    IEnumerator DelayTransitionState()
    {
        yield return new WaitForSeconds(0.5f);
        TransitionState(EnemyStateTypes.LaunchCannon); 
    }
    public void LaunchCannon()
    {
        StartCoroutine(AttackCoroutine());
    }
    
    private void SpawnCannon()
    {
        fireObj.SetActive(true);
        StartCoroutine(GenerateCannonPrefab());
    }

    IEnumerator GenerateCannonPrefab()
    {
        yield return new WaitForSeconds(0.4f);
        fireObj.SetActive(false);
        _cannonBallObj=ObjectPool.Spawn(Respath.CannonballPrefab, ObjectPool.Instance.transform, fireObj.transform.position);
        _cannonBallObj.GetComponent<CannonBall>().InitCannonBall(targetPosition);
    }
    private IEnumerator AttackCoroutine()
    {
        
        // 找到玩家的位置
        targetPosition= _player.position;
        //标出玩家位置
        var targetPositonObj=ObjectPool.Spawn(Respath.WarningLocation, ObjectPool.Instance.transform, targetPosition);
        // 等待前摇时间
        yield return new WaitForSeconds(preAttackTime);
        
        // 发射炮弹
        for (int i = 0; i < numberOfBullets; i++)
        {
            SpawnCannon();
            yield return new WaitForSeconds(0.5f); // 每隔一段时间发射一颗炮弹
            targetPositonObj.Recycle();
        }
        
    }


}
