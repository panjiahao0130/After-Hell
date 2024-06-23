using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    /// <summary>
    /// 攻击时更新生命值栏
    /// </summary>
    public event Action<int, int> UpdateHealthBarOnAttack;
    //角色数据 一直会变
    public CharacterData_SO characterData;
    //角色基础数据，不包括装备效果
    public CharacterData_SO baseCharacterData;
    //模板数据 怪物要设置这个模板数据 不然所有的每个种类怪物共用一个数据 会一刀死
    public CharacterData_SO templateData;
    
    //攻击数据,一直在变 然后保存在json里
    public AttackData_SO attackData; 
    //角色攻击数据，没有武器的状态
    public AttackData_SO baseAttackData;
    //基础攻击数据模板
    public AttackData_SO templateAttackData;
    
    //基础的动画
    //private RuntimeAnimatorController baseAnimator;

    private void Awake()
    {
        if (templateData!=null)
        {
            baseCharacterData = Instantiate(templateData);
            characterData = Instantiate(baseCharacterData);
        }

        if (templateAttackData!=null)
        {
            baseAttackData = Instantiate(templateAttackData);
            attackData = Instantiate(baseAttackData);
        }
        //runtimeAnimatorController就是一开始的animator controller里的动画
        //baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }
    
    #region ReadData from data_SO
    public int MaxHealth
    {
        get { if (characterData!=null) return characterData.maxHealth; return 0; }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData!=null) return characterData.currentHealth; return 0; }
        set { characterData.currentHealth = value; }
    }
    #endregion

    #region Character Combat

    /// <summary>
    /// 受到伤害 受到伤害 受到伤害  调用者是受伤的那一方 
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender">之后加defender</param>
    public void TakeDamage(CharacterStats attacker)
    {
        int damage = attacker.CurrentDamage();
        //当前血量 防止小于0
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        // 经验update
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);
        //update UI
    }
    /// <summary>
    /// 受到伤害 这是受到子弹伤害 调用者是受伤的那一方 
    /// </summary>
    /// <param name="damage">收到的伤害值</param>
    public void TakeDamage(int damage)
    {
        //当前血量 防止小于0
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        // 经验update
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);
        //update UI
    }
    
    private int CurrentDamage()
    {
        int coreDamage = attackData.attackDamage;
        return coreDamage;
    }

    #endregion
    
    
}
