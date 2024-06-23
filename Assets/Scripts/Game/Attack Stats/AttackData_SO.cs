using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack",menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Header("Attack Info")]
    [Tooltip("普通攻击范围")]
    public float attackRange;
    [Tooltip("攻击间隔")]
    public float coolDown;
    [Tooltip("攻击力")]
    public int attackDamage;
    
    
}
