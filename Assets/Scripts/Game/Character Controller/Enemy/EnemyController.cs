using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(CharacterStats),typeof(Collision2D),typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    public EnemyType enemyType;
    [Tooltip("状态转换等待时间")]
    public float transitionStateWaitingTime;
    public IState currentState;
    protected Dictionary<EnemyStateTypes, IState> states = new Dictionary<EnemyStateTypes, IState>();
    
    protected Transform _player; // 玩家对象
    protected PlayerController _playerController;
    [HideInInspector]
    public Rigidbody2D _rg2d;
    protected CharacterStats _characterStats;
    [HideInInspector]
    public Vector2 _targetDirection; //目标方向

    protected Transform _transform;
    private float currentScaleX;
    
    
    /// <summary>
    /// 初始化敌人控制器
    /// </summary>
    private void InitController()
    {
        // 获取玩家的位置
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _playerController = _player.GetComponent<PlayerController>();
        }
        _transform = transform;
        currentScaleX = _transform.localScale.x;
        _rg2d = GetComponent<Rigidbody2D>();
        _characterStats = GetComponent<CharacterStats>();

    }
    /// <summary>
    /// 初始化状态字典
    /// </summary>
    protected virtual void InitializeStatesDictionary()
    {
        states.Add(EnemyStateTypes.Stop, new StopState(this));
        states.Add(EnemyStateTypes.Idle, new IdleState(this));
        states.Add(EnemyStateTypes.Death, new DeathState(this));
    }
    
    public virtual void Awake()
    {
        InitController();
        SaveManager.Instance.SaveSceneName();
    }
     
    private  void Start() 
    {
        
    }

    private void OnEnable()
    {
        EnemySpawnManager.Instance.CurrentEnemyNum++;
        EnemySpawnManager.Instance.hasEnemy = true;
        //注册观察者
        GameManager.Instance.AddObserver(this);
        //初始化状态字典
        InitializeStatesDictionary();
        //初始状态为休息
        TransitionState(EnemyStateTypes.Idle);
        Debug.Log("当前场景为"+SaveManager.Instance.CurrentScene);
        if (SaveManager.Instance.CurrentScene=="Level1")
        {
            TransitionState(EnemyStateTypes.Stop);
        }
        _initialPosition = transform.position; 
    }

    public virtual void Update()
    {
        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj)
        {
            // 获取玩家的位置
            _player = obj.transform;
        }
        
        if (_player!=null)
        {
            _targetDirection = SetDirectionTowardsPlayer();
        }

        if (_characterStats.CurrentHealth<=0)
        {
            TransitionState(EnemyStateTypes.Death);
        }

        if (IsFacingLeft())
        {
            transform.localScale = new Vector3(-currentScaleX, _transform.localScale.y, _transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(currentScaleX, _transform.localScale.y, _transform.localScale.z);
        }
        
        //当前状态更新
        currentState.OnUpdate();
        
    }

    private void OnDisable()
    {
        if (!EnemySpawnManager.IsInitialized) return;
        EnemySpawnManager.Instance.CurrentEnemyNum--;
        if (!GameManager.IsInitialized) return;
        //移除观察者
        GameManager.Instance.RemoveObserver(this);
    }

    public void TransitionState(EnemyStateTypes type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }

    private bool IsFacingLeft()
    {
        // 如果x值为负数，则朝向左边
        return _targetDirection.x < 0;
    }

    private void OnDestroy()
    {
        
    }

    private Vector2 SetDirectionTowardsPlayer()
    {
        var target = (_player.position - transform.position).normalized;
        //_hasDirectionSet = true;
        return target;
    }
    
    public CharacterStats GetCharacterStats()
    {
        return _characterStats;
    }
    public void Knockback(Vector2 direction)
    {
        StartCoroutine(DoKnockback(direction));
    }

    [Tooltip("敌人被攻击时的击退时间")]
    public float knockbackDuration;
    private IEnumerator DoKnockback(Vector2 direction)
    {
        _rg2d.velocity = Vector2.zero; // 清除当前的速度
        _rg2d.AddForce(direction , ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        _rg2d.velocity = Vector2.zero; // 确保在持续时间结束后停止移动
    }
    
    private Vector2 _initialPosition; // 初始位置
    [Tooltip("撞到墙或者玩家后在玩家区域停留的时间")]
    public float returnTime = 1f; // 返回敌人区域的等待时间
    /// <summary>
    /// 返回原位
    /// </summary>
    public void StartReturn()
    {
        StartCoroutine(ReturnToInitialPosition());
    }
    private IEnumerator ReturnToInitialPosition()
    {
        //撞到玩家状态改为休息
        TransitionState(EnemyStateTypes.Idle);
        /*Debug.Log("开始返回");*/
        yield return new WaitForSeconds(returnTime);
        transform.position = _initialPosition;
        _rg2d.velocity = Vector2.zero;
    }


    
    public void GameWinEndNotify()
    {
        TransitionState(EnemyStateTypes.Stop);
    }
    /// <summary>
    /// 角色死亡通知
    /// </summary>
    public void GameFailEndNotify()
    {
        TransitionState(EnemyStateTypes.Stop);
    }
}