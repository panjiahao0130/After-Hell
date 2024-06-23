using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeThrowedEnemy : MonoBehaviour
{
    public float parabolicDuration = 3f; // 抛物线运动的持续时间
    private Vector3 startPosition; // 起始位置
    private Vector3 parabolicEndPosition; // 抛物线结束位置
    private float parabolicElapsedTime = 0f; // 抛物线运动已消耗的时间
    public float parabolicHeight = 2f; // 抛物线的高度
    private EnemyController _enemyController;
    public General _myBoss;
    private bool _hasBeIdle = false;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    private void Start()
    {
        startPosition = transform.position;
        // 随机决定抛物线结束位置是向左还是向右
        parabolicEndPosition = startPosition + new Vector3(RandomNumberGenerator.GenerateRandomFloat(-5f,-3f,3f,5f), Random.Range(-5f,-2f), 0f); // 抛物线结束位置，可以根据需要调整
    }

    public void SetBoss(General general)
    {
        _myBoss = general;
    }
    
    void Update()
    {
        if (_enemyController.GetCharacterStats().CurrentHealth<=0)
        {
            Debug.Log("死一个");
            _myBoss.OnEnemyKilled();
        }
        // 抛物线运动
        parabolicElapsedTime += Time.deltaTime;
        float t = parabolicElapsedTime / parabolicDuration;
        if (parabolicElapsedTime<parabolicDuration)
        {
            // 使用插值计算抛物线轨迹
            float height = Mathf.Sin(Mathf.PI * t) * parabolicHeight;
            Vector3 newPosition = Vector3.Lerp(startPosition, parabolicEndPosition, t);
            newPosition.y += height;
            transform.position = newPosition;
        }
        if (!_hasBeIdle)
        {
            _hasBeIdle = true;
            GetComponent<EnemyController>().TransitionState(EnemyStateTypes.Idle);
        }

        
        
    }
    
}
