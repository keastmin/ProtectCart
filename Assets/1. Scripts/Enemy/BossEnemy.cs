using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Bullet")]
    public GameObject _bullet;
    public float _bulletSpeed = 2f;
    public List<Transform> _spawnPoints = new List<Transform>();
    public float _attackTimer = 6f;
    public float _nextAttackTime = 0.5f;
    public int _attackCount = 4;
    private float _currentAttackTimer = 0f;

    [SerializeField] private Animator _animator;

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
            int randomPos = UnityEngine.Random.Range(0, _spawnPoints.Count);
            GameObject go = Instantiate(_bullet, _spawnPoints[randomPos].position, Quaternion.identity);
            EnemyBullet enemyBulet = go.GetComponent<EnemyBullet>();
            enemyBulet.BulletType = 1;
            enemyBulet.bulletSpeed = _bulletSpeed;
            yield return new WaitForSeconds(_nextAttackTime);
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
        Debug.Log("Boss Enemy is Dead");
        _animator.SetTrigger("Die");
        _isDead = true;
        GetComponent<Rigidbody>().isKinematic = true;

        base.Die();
    }
}
