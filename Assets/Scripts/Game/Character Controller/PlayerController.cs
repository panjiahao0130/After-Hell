using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("玩家的一些基础属性")]
    [Tooltip("玩家移动速度")]
    public float moveSpeed = 5f;
    [Tooltip("玩家冲刺速度")]
    public float dashSpeed = 5;
    
    [Tooltip("冲刺Cd")]
    public float dashCd = 3;

    /*[Tooltip("攻击间隔")]
    /*public float attackCd = 0.5f;
    private float _attackTimer=0f;#1#*/
    
    [Tooltip("攻击持续时间")]
    public float shootingTime = 2f;
    private float _shootingTimer = 0f;

    [Tooltip("使用反弹的冷却时间")]
    public float reBoundCd = 2f;
    private float _reBoundTimer = 0f;

    private SectorRange _attackSectorRange;
    private GameObject _reboundRange;
    
    //攻击范围的扇形角度
    private float _attackAngle = 60f;
    //攻击范围扇形半径
    private float _attackRadius = 5f;
    
    //是否正在冲刺
    
    private bool _isDashing;
    private GameObject _DashingAnimObj;
    
    private Rigidbody2D _rg2d;
    private LayerMask _playerAreaLayer;
    private Transform _transform;
    //角色朝向
    private Vector2 _playerDirection;
    //储存鼠标世界坐标
    private Vector2 _mouseWorldPos;
    private Vector2 _currentMousePosition;

    //拿箭头和范围的应用
    private GameObject _lineRender;
   // private SectorRange _sectorRange;
    
    private CharacterStats _characterStats;
    //玩家的状态 初始状态为一般状态
    private PlayerStates _playerStates=PlayerStates.General;

    private bool _isDead;
    //是否闪避成功
    private bool _isEscape;
    public AudioClip _bgAudioClip;
    public AudioClip _shootClip;
    public AudioClip _deathClip;
    private void Awake()
    {
        _rg2d=GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _lineRender = _transform.Find("Line").gameObject;
        _DashingAnimObj = transform.Find("DashAnimation").gameObject;
        
        _playerAreaLayer = LayerMask.GetMask("PlayerArea");
        _enemyLayer = LayerMask.GetMask("Enemy");
        _bulletLayer = LayerMask.GetMask("Bullet");
        _shieldLayer = LayerMask.GetMask("Shield");
        _characterStats = GetComponent<CharacterStats>();

        if (SceneManager.GetActiveScene().name=="Level8_Boss3")
        {
            _characterStats.MaxHealth = 80;
            _characterStats.CurrentHealth = 80;
        }
    }

    private void Start()
    {
        _attackSectorRange = transform.Find("AttackSectorRange").GetComponent<SectorRange>();
        _attackSectorRange.InitSector();
        _reboundRange = transform.Find("ReboundRange").gameObject;
        _attackAngle = _attackSectorRange.Angle;
        _attackRadius = _attackSectorRange.Radius * _transform.localScale.x;
        _lineRender.SetActive(false);
        _DashingAnimObj.SetActive(false);
        _attackSectorRange.HideSector();
        _reboundRange.SetActive(true);
        
    }

    private void OnEnable()
    {
        //注册角色数据
        GameManager.Instance.RegisterPlayer(_characterStats);
        AudioManager.instance.StopAllTracks();
        AudioManager.instance.StopAllTracks();
        AudioManager.instance.PlayTrack(_bgAudioClip, channel: 0, startingVolume: 1);
    }

    private void OnDisable()
    {
        AudioManager.instance.StopAllTracks();
        AudioManager.instance.StopAllTracks();
    }

    void Update()
    {
        _isDead = _characterStats.CurrentHealth == 0;
        if (_isDead)
        {
            _playerStates = PlayerStates.Dead;
        }
        //获取鼠标世界坐标
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        //计算角色到鼠标的方向矢量
        _playerDirection= (_mouseWorldPos-(Vector2)_transform.position).normalized;
        //使扇形攻击范围指向鼠标
        _attackSectorRange.transform.up = _playerDirection;
        _reboundRange.transform.up = _playerDirection;
        
        SwitchPlayStates();
    }

    public void SetPlayerStates(PlayerStates states)
    {
        _playerStates = states;
    }

    private void SwitchPlayStates()
    {
        switch (_playerStates)
        {
            case PlayerStates.General:
                _lineRender.SetActive(true);
                _attackSectorRange.HideSector();
                // 获取用户输入
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                // 计算移动方向
                Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
                // 移动角色
                _transform.position += moveDirection * moveSpeed * Time.deltaTime;
                
                // 满足可以冲刺条件，冲刺冷却时间，检测鼠标在玩家区域内，检测Shift键的按下事件,
                if (Input.GetKeyDown(KeyCode.LeftShift)&&IsMouseInPlayerArea(_mouseWorldPos)) 
                {
                    _currentMousePosition = _mouseWorldPos;
                    //冲刺之前将闪避成功标志改为true
                    _isEscape = true;
                    // 执行冲刺操作
                    DashToMousePosition(_playerDirection);
                }
                
                if (_isDashing)
                {
                    float distanceToMouse = Vector3.Distance(transform.position, _currentMousePosition);
                    if (distanceToMouse<0.5f)
                    {
                        _rg2d.velocity=Vector2.zero;
                        _isDashing = false;
                        _DashingAnimObj.SetActive(false);
                        //闪避结束之后判断是否闪避成功
                        if (_isEscape)
                        {
                            _playerStates = PlayerStates.ShootingTime;
                            _shootingTimer = shootingTime;
                        }
                    }
                }
                _reBoundTimer -= Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Space)&&_reBoundTimer<=0)
                {
                    ReboundBullet();
                }
                break;
            case PlayerStates.ShootingTime:
                _shootingTimer -= Time.deltaTime;
                _lineRender.SetActive(false);
                _attackSectorRange.ShowSector();
                if (Input.GetMouseButtonDown(0))
                {
                    //todo 播放散弹枪音效
                    AudioManager.instance.PlaySoundEffect(_shootClip);
                    Attack();
                    _playerStates = PlayerStates.General;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    _playerStates = PlayerStates.General;
                }

                if (_shootingTimer<=0)
                {
                    if (_playerStates!=PlayerStates.Dead)
                    {
                        _playerStates = PlayerStates.General;
                    }
                }
                break;
            case PlayerStates.Dead:
                Debug.Log("哥们死了");
                AudioManager.instance.PlaySoundEffect(_deathClip);
                //todo 播放死亡音效
                //通知所有观察者角色已经死亡
                if (GameManager.Instance.IsGameOver==false)
                {
                    GameManager.Instance.IsGameOver = true;
                    GameManager.Instance.NotifyObservers(GameOverType.Fail);
                }
                _lineRender.SetActive(false);
                _attackSectorRange.HideSector();
                _reboundRange.SetActive(false);
                Die();
                break;
        }
    }

    public CharacterStats GetCharacterStats()
    {
        return _characterStats;
    }

    private void DashToMousePosition(Vector3 targetPosition)
    {
        _isDashing = true;
        _rg2d.velocity = targetPosition*dashSpeed;
        _DashingAnimObj.transform.right = targetPosition;
        _DashingAnimObj.SetActive(true);
    }

    private bool IsMouseInPlayerArea(Vector2 position)
    {
        // 射线检测或碰撞检测，判断鼠标位置是否在玩家区域内
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, _playerAreaLayer);
        return hit.collider != null;
    }
   
    //击退时间
    [Tooltip("玩家被敌人攻击时的击退时间")]
    public float knockbackDuration = 0.5f;
    //击退距离
    [Tooltip("攻击对敌人的击退距离 包括子弹打到敌人的击退和玩家攻击敌人的击退")]
    public float knockbackDistance = 20f;

    /// <summary>
    /// 被击退
    /// </summary>
    /// <param name="direction"></param>
    public void Knockback(Vector2 direction)
    {
        StartCoroutine(DoKnockback(direction));
    }

    private IEnumerator DoKnockback(Vector2 direction)
    {
        //被撞到了说明没有闪避成功
        _isEscape = false;
        _rg2d.velocity = Vector2.zero; // 清除当前的速度
        _rg2d.AddForce(direction , ForceMode2D.Impulse);
        
        //打断角色冲刺动画
        _DashingAnimObj.SetActive(false);
        yield return new WaitForSeconds(knockbackDuration);

        _rg2d.velocity = Vector2.zero; // 确保在持续时间结束后停止移动
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    
    private LayerMask _enemyLayer;
    private LayerMask _bulletLayer;
    private LayerMask _shieldLayer;
    // 在场景中绘制扇形范围的可视化表示
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);

        // 计算扇形的起始角度和结束角度
        float startAngle = transform.eulerAngles.z - _attackAngle / 2f;
        float endAngle = transform.eulerAngles.z + _attackAngle / 2f;

        // 绘制扇形范围的边界
        Vector2 fanDirectionStart = Quaternion.Euler(0, 0, startAngle) * Vector2.right.normalized;
        Vector2 fanDirectionEnd = Quaternion.Euler(0, 0, endAngle) * Vector2.right.normalized;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)fanDirectionStart * _attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)fanDirectionEnd * _attackRadius);
    }
    /// <summary>
    /// 对敌人进行公假
    /// </summary>
    private void Attack()
    {
        // 使用 LayerMask 过滤出敌人和子弹
        var enemyColliders = Physics2D.OverlapCircleAll(_transform.position, _attackRadius, _enemyLayer);
        var bulletColliders = Physics2D.OverlapCircleAll(_transform.position, _attackRadius, _bulletLayer);
        var shieldCollider = Physics2D.Raycast(_transform.position, _transform.up, _attackRadius, _shieldLayer);

        if (!shieldCollider)
        {
            // 处理对敌人的攻击
            foreach (Collider2D col in enemyColliders)
            {
                Vector3 direction = (col.transform.position - transform.position).normalized;
                HandleAttack(col, direction);
            }

            // 处理对子弹的攻击
            foreach (Collider2D col in bulletColliders)
            {
                Vector3 direction = (col.transform.position - transform.position).normalized;
                HandleBulletAttack(col, direction);
            }
        }
        else
        {
            var shield = shieldCollider.collider.GetComponent<Shield>();
            if (shield)
            {
                if (shield.isBreakable)
                {
                    shield.TakeDamage();
                }
                else
                {
                    
                }
            }
        }
        
        
    }

    // 处理子弹的攻击
    private void HandleBulletAttack(Collider2D col, Vector3 direction)
    {
        float angle = Vector3.Angle(_playerDirection, direction);
        if (angle <= _attackAngle / 2)
        {
            Bullet bullet = col.GetComponent<Bullet>();
            if (bullet != null)
            {
                // 处理子弹被攻击的逻辑
                bullet.BeAttacked(direction);
            }
        }
    }

    // 处理对敌人的攻击
    private void HandleAttack(Collider2D col, Vector3 direction)
    {
        float angle = Vector3.Angle(_playerDirection, direction);
        if (angle <= _attackAngle / 2)
        {
            EnemyController enemy = col.GetComponent<EnemyController>();
            if (enemy != null)
            {
                if (enemy is General)
                {
                    return;
                }
                enemy.GetCharacterStats().TakeDamage(_characterStats.attackData.attackDamage);
                enemy.Knockback(direction * knockbackDistance);
                if (enemy is Rampager)
                {
                    enemy.StartReturn();
                }
                _rg2d.velocity = Vector2.zero;
            }
        }
    }

    private void ReboundBullet()
    {
        _reBoundTimer = reBoundCd;
        var bulletColliders = Physics2D.OverlapCircleAll(transform.position, 2, _bulletLayer);
        // 处理子弹的攻击
        foreach (Collider2D col in bulletColliders)
        {
            col.GetComponent<Bullet>().BounceToEnemy();
        }
    }
    
    
}