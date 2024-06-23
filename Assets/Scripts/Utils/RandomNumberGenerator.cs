using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomNumberGenerator
{

    public static int GenerateRandomInt(int min1, int max1, int min2, int max2)
    {
        // 随机选择区间
        if (Random.value < 0.5f)
        {
            return Random.Range(min1, max1);
        }
        else
        {
            return Random.Range(min2, max2);
        }
    }

    public static float GenerateRandomFloat(float min1, float max1, float min2, float max2)
    {
        // 随机选择区间
        if (Random.value < 0.5f)
        {
            return Random.Range(min1, max1);
        }
        else
        {
            return Random.Range(min2, max2);
        }
    }
}
