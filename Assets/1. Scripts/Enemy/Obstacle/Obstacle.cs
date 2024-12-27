using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Obstacle : MonoBehaviour
{
    [SerializeField] protected Transform enemyContainer;
    [SerializeField] protected float health;
    protected float currentHealth;
    [SerializeField] protected float damageThreshold;
    protected bool isDead;

    protected bool CalculateDamage(out float damage, Collision collision)
    {
        damage = 0f;
        if(collision.rigidbody != null)
        {
            damage = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
            //Debug.Log(damage);
            return true;
        }
        return false;
    }

    protected void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.Log($"{health} / {currentHealth}");
    }

    protected virtual void Die()
    {
        Debug.Log("Object is Dead");
    }
}
