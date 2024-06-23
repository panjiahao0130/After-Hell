using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManSergeant : EnemyController
{
    private SectorRange _sectorRange;
    private LayerMask _pLayerMask; // 敌人所在的层级
    private float fanAngle ; // 扇形的角度
    private float fanRadius; // 扇形的半径
    [Tooltip("撞击速度")]
    public float moveSpeed = 5f; // 移动速度

    [Tooltip("毒雾的持续时间")]
    public float PoisonousDuration = 5;

    [HideInInspector]
    public bool HasDash;

    public GameObject _smokeObj;
    
    protected override void InitializeStatesDictionary()
    {
        base.InitializeStatesDictionary();
        states.Add(EnemyStateTypes.Dash, new DashState(this));
        states.Add(EnemyStateTypes.LaunchPoisonousMist,new LaunchPoisonousMistState(this));
    }

    public override void Awake()
    {
        base.Awake();
        _sectorRange = transform.Find("AttackSectorRange").GetComponent<SectorRange>();
        _sectorRange.InitSector();
        _sectorRange.transform.up = transform.right;
        _sectorRange.gameObject.SetActive(false);
        fanAngle = _sectorRange.Angle;
        fanRadius = _sectorRange.Radius * _transform.localScale.x;
        _pLayerMask = LayerMask.GetMask("Player");
        _smokeObj = transform.GetChild(1).gameObject;
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
            //撞到玩家状态改为休息
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
        TransitionState(EnemyStateTypes.LaunchPoisonousMist);
        
    }
    

    public void LaunchPoisonousMist()
    {
        StartCoroutine(SkillForward());
    }
    
    IEnumerator SkillForward()
    {
        //todo 技能前摇表现
        yield return new WaitForSeconds(1f);
        SpawnPoisonousMist();
    }
    private void SpawnPoisonousMist()
    {
        //todo 生成毒雾预制体 先用这个范围代替
        _sectorRange.gameObject.SetActive(true);
    }

    public void StopLaunchPoisonousMist()
    {
        _sectorRange.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 对玩家造成伤害 
    /// </summary>
    public void AttackPlayer()
    {
        // 使用 LayerMask 过滤出敌人和子弹
        var playerCollider2Ds = Physics2D.OverlapCircleAll(_transform.position, fanRadius, _pLayerMask);
        // 处理对玩家的攻击
        foreach (Collider2D col in playerCollider2Ds)
        {
            Vector3 direction = (col.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(_sectorRange.transform.up, direction);
            if (angle <= fanAngle / 2)
            {
                PlayerController player = col.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.GetCharacterStats().TakeDamage(_characterStats.attackData.attackDamage);
                }
            }
        }
    }
    // 在场景中绘制扇形范围的可视化表示
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fanRadius);

        // 计算扇形的起始角度和结束角度
        float startAngle = transform.eulerAngles.z - fanAngle / 2f;
        float endAngle = transform.eulerAngles.z + fanAngle / 2f;

        // 绘制扇形范围的边界
        Vector2 fanDirectionStart = Quaternion.Euler(0, 0, startAngle) * Vector2.right.normalized;
        Vector2 fanDirectionEnd = Quaternion.Euler(0, 0, endAngle) * Vector2.right.normalized;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)fanDirectionStart * fanRadius);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)fanDirectionEnd * fanRadius);
    }
    
}
