using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rampager : EnemyController
{
    [Tooltip("撞击速度")]
    public float moveSpeed = 5f; // 移动速度
    
    protected override void InitializeStatesDictionary()
    {
        base.InitializeStatesDictionary();
        states.Add(EnemyStateTypes.Dash, new DashState(this));
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
            _rg2d.velocity=Vector2.zero;
            //撞到玩家状态改为休息
            TransitionState(EnemyStateTypes.Idle);
            StartReturn();
        }

        if (other.gameObject.CompareTag("Boundary"))
        {
            TransitionState(EnemyStateTypes.Idle);
           StartReturn();
        }
    }

    
    
}
