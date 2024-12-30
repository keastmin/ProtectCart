using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObstacle : Obstacle
{
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
        Debug.Log("Small Obstacle is Dead");

        Destroy(this.gameObject);
    }
}
