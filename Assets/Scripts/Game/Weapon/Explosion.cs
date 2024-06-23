using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void DelayRecycle()
    {
        StartCoroutine(StartDelayRecycle());
    }

    IEnumerator StartDelayRecycle()
    {
        yield return new WaitForSeconds(1f);
        ObjectPool.Recycle(gameObject);
    }
}
