using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        Boss
    }
    public EnemyType Type;

    [SerializeField] protected float health = 30f;
    [SerializeField] protected float damageThreshold = 2f;

    public event Action<Enemy> OnEnemyDied;

    public bool IsCanAttack;
    protected float currentHealth;
    protected bool _isDead;

    protected bool CalculateDamage(out float damage, Collision collision)
    {
        damage = 0f;
        if (collision.rigidbody != null)
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
        //Debug.Log("Enemy is Dead");
        OnEnemyDied?.Invoke(this);
    }
}
