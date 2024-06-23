using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Tooltip("是否可被打破")]
    public bool isBreakable;

    [SerializeField]
    [Tooltip("需要被攻击次数")]
    private int beHitNum;

    //当前被攻击的次数
    private int _currentHitNum = 0;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void TakeDamage()
    {
        _currentHitNum++;
        Debug.Log("被打了"+_currentHitNum+"次");
        // 遍历所有子物体
        for (int i = 0; i < _transform.childCount; i++)
        {
            // 获取第 i 个子物体的 Transform 组件
            SpriteRenderer child = _transform.GetChild(i).GetComponent<SpriteRenderer>(); 
            child.color= new Color(0,0,0,(1-_currentHitNum*1.0f/beHitNum));
        }
        if (_currentHitNum==beHitNum)
        {
            gameObject.SetActive(false);
        }
    }
}
