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

            if (currentHealth <= 0)
            {
                isDead = true;
                Die();
            }
        }
    }

    protected override void Die()
    {
        Debug.Log("Small Obstacle is Dead");

        // 원래 오브젝트 파괴
        Destroy(this.gameObject);
    }
}
