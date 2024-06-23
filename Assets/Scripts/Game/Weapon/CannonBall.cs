using System;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private Vector3 target; // 目标点
    public float parabolicDuration = 2f; // 抛物线运动的持续时间
    public float linearSpeed = 5f; // 直线运动的速度

    private Vector3 startPosition; // 起始位置
    private Vector3 parabolicEndPosition; // 抛物线结束位置
    private float parabolicElapsedTime = 0f; // 抛物线运动已消耗的时间
    private bool isParabolicPhase = true; // 是否处于抛物线阶段
    public int cannonBallDamage;

    void Start()
    {
        startPosition = transform.position;
        parabolicEndPosition = startPosition + new Vector3(-3f, 2f, 0f); // 抛物线结束位置，可以根据需要调整
    }

    void Update()
    {
        if (isParabolicPhase)
        {
            // 抛物线运动
            parabolicElapsedTime += Time.deltaTime;
            float t = parabolicElapsedTime / parabolicDuration;
            if (t >= 1f)
            {
                t = 1f;
                isParabolicPhase = false;
            }

            // 使用插值计算抛物线轨迹
            float height = Mathf.Sin(Mathf.PI * t) * parabolicEndPosition.y;
            Vector3 newPosition = Vector3.Lerp(startPosition, parabolicEndPosition, t);
            newPosition.y += height;
            transform.position = newPosition;
        }
        else
        {
            // 直线运动
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * linearSpeed * Time.deltaTime;

            // 检查是否到达目标点
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                // 到达目标点，销毁炮弹
                ObjectPool.Recycle(gameObject);
                //生成爆炸特效
                var explosionObj=ObjectPool.Spawn(Respath.ExplosionPrefab, ObjectPool.Instance.transform, transform.position);
                explosionObj.GetComponent<Explosion>().DelayRecycle();
            }
        }
    }

    public void InitCannonBall(Vector2 targetPosition)
    {
        target = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("爆炸");
            other.GetComponent<PlayerController>().GetCharacterStats().TakeDamage(cannonBallDamage);
        }
    }
}

