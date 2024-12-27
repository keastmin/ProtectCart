using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public float _attackTimer = 4f;
    private float _currentAttackTimer = 0f;


    private void Awake()
    {
        Type = EnemyType.Normal;
        currentHealth = health;
    }

    private void Update()
    {
        if (!_isDead && IsCanAttack)
        {
            _currentAttackTimer += Time.deltaTime;
            if(_currentAttackTimer >= _attackTimer)
            {
                _currentAttackTimer = 0f;
                GameObject go = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
                EnemyBullet enemyBulet = go.GetComponent<EnemyBullet>();
                enemyBulet.BulletType = 0;
                enemyBulet.bulletSpeed = bulletSpeed;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float damage;
        bool isCanDamage = CalculateDamage(out damage, collision);

        if (damage > damageThreshold && isCanDamage && !_isDead)
        {
            ApplyDamage(damage - damageThreshold);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    protected override void Die()
    {
        Debug.Log("Normal Enemy is Dead");
        _isDead = true;
        GetComponent<Rigidbody>().isKinematic = true;

        base.Die();

        Destroy(this.gameObject);
    }
}
