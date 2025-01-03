using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Obstacle : MonoBehaviour
{
    [SerializeField] protected float health;
    protected float currentHealth;
    [SerializeField] protected float damageThreshold;
    [SerializeField] protected GameObject smokeParticle;
    [SerializeField] protected GameObject hitParticle;
    protected bool isDead;

    protected bool CalculateDamage(out float damage, Collision collision)
    {
        damage = 0f;
        if(collision.rigidbody != null)
        {
            damage = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
            return true;
        }
        return false;
    }

    protected void ApplyDamage(float damage)
    {
        currentHealth -= damage;
    }

    protected virtual void Die()
    {
        Debug.Log("Object is Dead");
    }
}
