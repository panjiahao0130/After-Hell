using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家的状态
/// </summary>
public enum PlayerStates
{
    /// <summary>
    /// 一般状态
    /// </summary>
    General,
    /// <summary>
    /// 射击时间
    /// </summary>
    ShootingTime,
    /// <summary>
    /// 死亡状态
    /// </summary>
    Dead,
}

/// <summary>
/// 敌人的状态类型
/// </summary>
public enum EnemyStateTypes
{
    /// <summary>
    /// 停止状态 
    /// </summary>
    Stop,
    /// <summary>
    /// 一般休息状态
    /// </summary>
    Idle,
    /// <summary>
    /// 死亡状态
    /// </summary>
    Death,
    /*/// <summary>
    /// 受击状态
    /// </summary>
    Hit,*/
    /// <summary>
    /// 冲撞状态
    /// </summary>
    Dash,
    /// <summary>
    /// 发射子弹
    /// </summary>
    LaunchBullets,
    /// <summary>
    /// 喷射毒雾
    /// </summary>
    LaunchPoisonousMist,
    /// <summary>
    /// 扔士兵
    /// </summary>
    ThrowSoldiers,
    /// <summary>
    /// 机枪射击
    /// </summary>
    MachineGunShooting,
    /// <summary>
    /// 发射火炮
    /// </summary>
    LaunchCannon,
    /// <summary>
    /// 复制玩家的行动
    /// </summary>
    CopyPlayer,
}

/// <summary>
/// 敌人类型
/// </summary>
public enum EnemyType
{
    /// <summary>
    /// 射击者
    /// </summary>
    Sharpshooter,
    /// <summary>
    /// 撞击者
    /// </summary>
    Rampager,
    /// <summary>
    /// 鱼人军士
    /// </summary>
    FishManSergeant,
    /// <summary>
    /// 战争机器
    /// </summary>
    WarMachine,
    /// <summary>
    /// 将军
    /// </summary>
    General,
    /// <summary>
    /// 战争机器Plus
    /// </summary>
    WarMachinePlus,
    /// <summary>
    /// 自我心魔
    /// </summary>
    SelfDemon

}

/// <summary>
/// 关卡结束类型
/// </summary>
public enum GameOverType
{
    Win,
    Fail,
}

public enum LevelType
{
    Common,
    Boss,
}
