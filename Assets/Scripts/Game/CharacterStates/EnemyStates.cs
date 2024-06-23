
using System;
using System.Collections;
using UnityEngine;
public class StopState:IState
{
    private EnemyController _enemyController;
   
    public StopState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }
    public void OnEnter()
    {
        Debug.Log("当前是停滞状态");
        //啥都不做
        _enemyController._rg2d.velocity=Vector2.zero;
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
public class IdleState:IState
{
    private EnemyController _enemyController;
    private float _timer;
    private float _waitTime;
    public IdleState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _waitTime = _enemyController.transitionStateWaitingTime;
    }
    public void OnEnter()
    {
        Debug.Log("当前是idle状态");
    }

    public void OnUpdate()
    {
        _timer += Time.deltaTime;
        switch (_enemyController.enemyType)
        {
            case EnemyType.Sharpshooter:
                if (_timer>_waitTime)
                {
                    _enemyController.TransitionState(EnemyStateTypes.LaunchBullets);
                }
                break;
            case EnemyType.Rampager:
                if (_timer>_waitTime)
                {
                    _enemyController.TransitionState(EnemyStateTypes.Dash);
                }
                break;
            case EnemyType.FishManSergeant:
                var fishMan = (FishManSergeant)_enemyController;
                if (fishMan.HasDash)
                {
                    //这个五秒是转换到boss特殊的喷射毒雾状态的等待时间
                    if (_timer>5)
                    {
                        fishMan.TransitionState(EnemyStateTypes.LaunchPoisonousMist);
                    }
                }
                else
                {
                    if (_timer>_waitTime)
                    {
                        fishMan.TransitionState(EnemyStateTypes.Dash);
                        fishMan.HasDash = true;
                    }
                }
                
                break;
            case EnemyType.WarMachine:
                var warMachine = (WarMachine)_enemyController;
                if (_timer>_waitTime)
                {
                    warMachine.TransitionState(EnemyStateTypes.Dash);
                }
                
                break;
            case EnemyType.General:
                var general = (General)_enemyController;
                if (_timer>_waitTime)
                {
                    general.TransitionState(EnemyStateTypes.ThrowSoldiers);
                }
                break;
            case EnemyType.WarMachinePlus:
                break;
            case EnemyType.SelfDemon:
                break;
        }
    }

    public void OnExit()
    {
        _timer = 0;
    }
}
public class DeathState:IState
{
    private EnemyController _enemyController;
    private BeThrowedEnemy _beThrowedEnemy;
    
   
    public DeathState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }
    public void OnEnter()
    {
        if (_enemyController.transform.GetComponent<BeThrowedEnemy>())
        {
            _beThrowedEnemy = _enemyController.transform.GetComponent<BeThrowedEnemy>();
            _beThrowedEnemy._myBoss.OnEnemyKilled();
        }
        //todo 播放死亡音效
        Debug.Log(_enemyController.gameObject.name+"死了");
        ObjectPool.Recycle(_enemyController.gameObject);
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
public class DashState:IState
{
    private EnemyController _enemyController;
    private float _cdTimer;
    private bool _isDashing;
    private float _cd;
    private float _moveSpeed;
    private Vector2 _targetDirection;
   
    public DashState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _cd = _enemyController.GetCharacterStats().attackData.coolDown;
        if (enemyController is Rampager rampager)
        {
            _enemyController=rampager;
            _moveSpeed = rampager.moveSpeed;
        }
        else if (enemyController is FishManSergeant fishManSergeant)
        {
            _enemyController = fishManSergeant;
            _moveSpeed = fishManSergeant.moveSpeed;
        }
        else if (enemyController is WarMachine warMachine)
        {
            _enemyController = warMachine;
            _moveSpeed = warMachine.moveSpeed;
        }
        
        
    }
    public void OnEnter()
    {
        _isDashing = true;
        /*Debug.Log("现在是冲锋状态");*/
    }

    public void OnUpdate()
    {
        _cdTimer -= Time.deltaTime;
        if (_isDashing&&_cdTimer<=0)
        {
            _cdTimer = _cd;
            _targetDirection = _enemyController._targetDirection;
            MoveTowardsPlayer();
        }
    }

    public void OnExit()
    {
        _isDashing = false;
    }
    /// <summary>
    /// 撞向玩家
    /// </summary>
    private void MoveTowardsPlayer()
    {
        //Vector3 direction = (player.position - transform.up).normalized;
        //_enemyController.transform.right = _targetDirection;
        _enemyController.GetComponent<Rigidbody2D>().AddForce(_targetDirection*_moveSpeed,ForceMode2D.Impulse);
    }
    
}
public class LaunchBulletsState:IState
{
    private EnemyController _enemyController;
    //子弹对象
    private GameObject _bulletGameObject;
    
    
    [Tooltip("射击间隔")] 
    private float _shootingInterval;
    private float _shootingTimer;
    [Tooltip("子弹速度")]
    private float _bulletSpeed;
    [Tooltip("子弹伤害")]
    private int _bulletDamage;

    private Shooter_O _shooterO;
   
    public LaunchBulletsState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        if (enemyController is Sharpshooter sharpshooter)
        {
            _enemyController=sharpshooter;
            _shooterO = _enemyController.GetComponent<Shooter_O>();
            _shootingInterval = sharpshooter.shootingInterval;
            _bulletSpeed = sharpshooter.bulletSpeed;
            _bulletDamage = sharpshooter.bulletDamage;
        }
        else
        {
            //todo 其他有射击状态的敌人
        }
        
    }
    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        /*_shootingTimer -= Time.deltaTime;
        if (_shootingTimer<=0&&_enemyController._targetDirection!=Vector2.zero)
        {
            _enemyController.transform.up = _enemyController._targetDirection;
            //todo 生成预制体
            GameObject bulletObj = ObjectPool.Spawn(Respath.BulletPrefab,ObjectPool.Instance.transform,_enemyController.transform.position);
            bulletObj.GetComponent<Bullet>().InitBullet(_bulletSpeed,_bulletDamage,0.5f,_enemyController._targetDirection);
            _shootingTimer = _shootingInterval;
        }*/
        _shooterO.Attack();
    }

    public void OnExit()
    {
        
    }
}
public class LaunchPoisonousMistState:IState
{
    private FishManSergeant _fishManSergeant;
    private float _attackIntervalTimer;
    private float _duration;
    private float _poisonousTimer;
    public LaunchPoisonousMistState(EnemyController enemyController)
    {
        _fishManSergeant = (FishManSergeant)enemyController;
        _duration = _fishManSergeant.PoisonousDuration;
    }
    public void OnEnter()
    {
       Debug.Log("当前是喷毒状态");
       //_fishManSergeant.transform.right = _fishManSergeant._targetDirection;
       _fishManSergeant._smokeObj.SetActive(true);
       _fishManSergeant.LaunchPoisonousMist();
    }

    public void OnUpdate()
    {
        _poisonousTimer += Time.deltaTime;
        _attackIntervalTimer += Time.deltaTime;
        if (_attackIntervalTimer>0.5)
        {
            _attackIntervalTimer = 0;
            _fishManSergeant.AttackPlayer();
        }

        if (_poisonousTimer>_duration)
        {
            _fishManSergeant.TransitionState(EnemyStateTypes.Idle);
        }
    }

    public void OnExit()
    {
        _fishManSergeant.StopLaunchPoisonousMist();
        _fishManSergeant._smokeObj.SetActive(false);
        _poisonousTimer = 0;
    }
}
public class ThrowSoldiersState:IState
{
    private General _general;
    private float _spawnInterval;
    private float _timer;
    private int MaxEnemyNum = 3;
    private int _currentEnemyCount = 0;
   
    public ThrowSoldiersState(EnemyController enemyController)
    {
        _general = (General)enemyController;
        _spawnInterval = _general.spawnInterval;
    }
    public void OnEnter()
    {
      
    }

    public void OnUpdate()
    {
        
        if (_currentEnemyCount < MaxEnemyNum)
        {
            _timer += Time.deltaTime;
            if (_timer > _spawnInterval)
            {
                _general.SpawnEnemies(_currentEnemyCount);
                _currentEnemyCount++;
                _timer = 0;
            }
        }
        else
        {
            _general.TransitionState(EnemyStateTypes.Stop);
        }
        
        
    }

    public void OnExit()
    {
        
    }
}
public class MachineGunShootingState:IState
{
    private EnemyController _enemyController;
   
    public MachineGunShootingState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }
    public void OnEnter()
    {
       
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
public class LaunchCannonState:IState
{
    private WarMachine _warMachine;
    private float _attackIntervalTimer;
    private float _interval;
    public LaunchCannonState(EnemyController enemyController)
    {
        _warMachine = (WarMachine)enemyController;
        _interval = _warMachine.interval;
    }
    public void OnEnter()
    {
        Debug.Log("当前是发射炮弹状态状态");
    }

    public void OnUpdate()
    {
        _attackIntervalTimer += Time.deltaTime;
        if (_attackIntervalTimer>_interval)
        {
            _warMachine.LaunchCannon();
            _attackIntervalTimer = 0;
        }
        
    }

    public void OnExit()
    {
        
    }
}
public class CopyPlayerState:IState
{
    private EnemyController _enemyController;
   
    public CopyPlayerState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }
    public void OnEnter()
    {
       
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
