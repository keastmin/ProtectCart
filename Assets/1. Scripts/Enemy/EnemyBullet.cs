using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int BulletType = 0;
    public float bulletSpeed = 10f;
    [SerializeField] GameObject _particle;

    private Vector3 direction;
    private Collider targetCollider;
    private Rigidbody rb;

    private void Awake()
    {
        GameObject cart = GameObject.Find("Cart");
        rb = GetComponent<Rigidbody>();
        targetCollider = cart.GetComponent<Collider>();

        if (targetCollider == null)
        {
            Debug.LogError("Target Collider is not assigned!");
        }
    }

    private void Start()
    {
        InitializeBulletDirection();
    }

    private void InitializeBulletDirection()
    {
        if (targetCollider == null || rb == null)
        {
            Debug.LogWarning("TargetCollider or Rigidbody is missing!");
            return;
        }

        Vector3 targetPoint;

        if (BulletType == 1)
        {
            targetPoint = GetRandomPointInBounds();
        }
        else
        {
            targetPoint = GetCenterPointWithRandomY();
        }

        direction = (targetPoint - transform.position).normalized;
    }

    private Vector3 GetRandomPointInBounds()
    {
        Bounds bounds = targetCollider.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }

    private Vector3 GetCenterPointWithRandomY()
    {
        Bounds bounds = targetCollider.bounds;

        float centerX = 0;
        float centerZ = 0;
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(centerX, randomY, centerZ);
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Instantiate(_particle, collision.GetContact(0).point, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
