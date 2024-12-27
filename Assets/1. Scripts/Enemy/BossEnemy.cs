using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public float _attackTimer = 6f;
    public int _attackCount = 4;
    private float _currentAttackTimer = 0f;

    private void Awake()
    {
        Type = EnemyType.Boss;
        currentHealth = health;
    }

    private void Update()
    {
        if(!_isDead && IsCanAttack)
        {
            _currentAttackTimer += Time.deltaTime;
            if (_currentAttackTimer >= _attackTimer)
            {
                _currentAttackTimer = 0f;
                StartCoroutine(CreateBullet());
            }
        }
    }

    private IEnumerator CreateBullet()
    {
        for(int i = 0; i < _attackCount; i++)
        {
            GameObject go = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
            EnemyBullet enemyBulet = go.GetComponent<EnemyBullet>();
            enemyBulet.BulletType = 1;
            enemyBulet.bulletSpeed = bulletSpeed;
            yield return new WaitForSeconds(0.5f);
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
        Debug.Log("Boss Enemy is Dead");
        _isDead = true;
        GetComponent<Rigidbody>().isKinematic = true;

        base.Die();

        Destroy(this.gameObject);
    }
}
