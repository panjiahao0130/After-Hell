using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 射击怪 会不断发射子弹
/// </summary>
public class Sharpshooter : EnemyController
{
    [Tooltip("射击间隔")]
    public float shootingInterval=1;
    //private float _shootingTimer=0;
    [Tooltip("子弹速度")]
    public float bulletSpeed;
    [Tooltip("子弹伤害")]
    public int bulletDamage;
    
    [Tooltip("子弹的击退距离")]
    public int bulletKnockbackDuration;

    protected override void InitializeStatesDictionary()
    {
        base.InitializeStatesDictionary();
        states.Add(EnemyStateTypes.LaunchBullets, new LaunchBulletsState(this));
    }
    
}
