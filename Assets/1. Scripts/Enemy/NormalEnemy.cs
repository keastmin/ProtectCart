using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    [Header("Bullet")]
    public GameObject _bullet;
    public float _bulletSpeed = 2f;
    public Transform _spawnPoint;
    public float _attackTimer = 6f;
    private float _currentAttackTimer = 0f;

    [SerializeField] private Animator _animator;

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
                GameObject go = Instantiate(_bullet, _spawnPoint.position, Quaternion.identity);
                EnemyBullet enemyBulet = go.GetComponent<EnemyBullet>();
                enemyBulet.BulletType = 0;
                enemyBulet.bulletSpeed = _bulletSpeed;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float damage;
        bool isCanDamage = CalculateDamage(out damage, collision);

        if (!_isDead && collision.collider.CompareTag("Ground"))
        {
            currentHealth = 0;
            Die();
        }
        else if (damage > damageThreshold && isCanDamage && !_isDead)
        {
            ApplyDamage(damage - damageThreshold);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                _animator.Play("Hit");
            }
        }
    }

    protected override void Die()
    {
        Debug.Log("Normal Enemy is Dead");
        _animator.SetTrigger("Die");
        _isDead = true;
        GetComponent<Rigidbody>().isKinematic = true;

        base.Die();
    }
}
