using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    protected override void Fire()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        RaycastHit2D hit2D = Physics2D.Raycast(muzzlePos.position, direction, 30);

        GameObject bullet = ObjectPool.Spawn(bulletPrefab);
        LineRenderer tracer = bullet.GetComponent<LineRenderer>();
        tracer.SetPosition(0, muzzlePos.position);
        tracer.SetPosition(1, muzzlePos.position + (mousePosition - muzzlePos.position).normalized * 5);
        
    }
}