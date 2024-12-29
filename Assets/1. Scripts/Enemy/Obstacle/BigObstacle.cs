using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigObstacle : Obstacle
{
    [SerializeField] private GameObject _middleObstacle;

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
                isDead = true;
                Instantiate(smokeParticle, transform.position, Quaternion.identity);
                Die();
            }
        }
    }

    protected override void Die()
    {
        Debug.Log("Big Obstacle is Dead");

        // 현재 오브젝트의 Transform 정보
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        Vector3 originalScale = transform.localScale;

        // _middleObstacle의 크기 조정 (y 방향 절반)
        Vector3 middleScale = new Vector3(originalScale.x, originalScale.y / 2, originalScale.z);

        // 위쪽 절반 위치 계산
        Vector3 topPosition = originalPosition + transform.up * (originalScale.y / 4);

        // 아래쪽 절반 위치 계산
        Vector3 bottomPosition = originalPosition - transform.up * (originalScale.y / 4);

        // 위쪽 오브젝트 생성
        GameObject topObstacle = Instantiate(_middleObstacle, topPosition, originalRotation, transform.parent);
        topObstacle.transform.localScale = middleScale;

        // 아래쪽 오브젝트 생성
        GameObject bottomObstacle = Instantiate(_middleObstacle, bottomPosition, originalRotation, transform.parent);
        bottomObstacle.transform.localScale = middleScale;

        // 원래 오브젝트 파괴
        Destroy(this.gameObject);
    }
}

