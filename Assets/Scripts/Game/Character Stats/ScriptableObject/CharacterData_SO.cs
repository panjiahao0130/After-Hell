using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
     [Header("Stats Info")] 
     [Tooltip("最大血量")]
     public int maxHealth;
     [Tooltip("当前血量")]
     public int currentHealth;
     
     
}
