using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleObstacle : Obstacle
{
    [SerializeField] private GameObject _smallObstacle;

    private void Awake()
    {
        currentHealth = health;
        isDead = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float damage;
        bool isCanDamage = CalculateDamage(out damage, collision);

        if (damage > damageThreshold && isCanDamage && !isDead)
        {
            ApplyDamage(damage - damageThreshold);

            Vector3 pos = collision.GetContact(0).point;
            Instantiate(hitParticle, pos, Quaternion.identity);

            if (currentHealth <= 0)
            {
                Instantiate(smokeParticle, transform.position, Quaternion.identity);
                isDead = true;
                Die();
            }
        }
    }

    protected override void Die()
    {
        Debug.Log("Middle Obstacle is Dead");

        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        Vector3 originalScale = transform.localScale;

        Vector3 middleScale = new Vector3(originalScale.x, originalScale.y / 2, originalScale.z);

        Vector3 topPosition = originalPosition + transform.up * (originalScale.y / 2);

        Vector3 bottomPosition = originalPosition - transform.up * (originalScale.y / 2);

        GameObject topObstacle = Instantiate(_smallObstacle, topPosition, originalRotation, transform.parent);
        topObstacle.transform.localScale = middleScale;

        GameObject bottomObstacle = Instantiate(_smallObstacle, bottomPosition, originalRotation, transform.parent);
        bottomObstacle.transform.localScale = middleScale;

        Destroy(this.gameObject);
    }
}
