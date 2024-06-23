using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    /*[SerializeField] private bool isEnemyProjectile = false;*/
    [SerializeField] private float projectileRange = 10f;

    private Vector3 startPosition;

    private Rigidbody2D _rigidbody2D;
    private void Start() {
        startPosition = transform.position;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveProjectile();
        //DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange){
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    /*private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            ObjectPool.Recycle(gameObject);
        }
    }

    private void DetectFireDistance() {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange) {
            ObjectPool.Recycle(gameObject);
        }
    }*/

    private void MoveProjectile()
    {
        transform.Translate( Vector3.right* Time.deltaTime * moveSpeed);
    }
}