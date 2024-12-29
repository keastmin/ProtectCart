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

        // ���� ������Ʈ�� Transform ����
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        Vector3 originalScale = transform.localScale;

        // _middleObstacle�� ũ�� ���� (y ���� ����)
        Vector3 middleScale = new Vector3(originalScale.x, originalScale.y / 2, originalScale.z);

        // ���� ���� ��ġ ���
        Vector3 topPosition = originalPosition + transform.up * (originalScale.y / 4);

        // �Ʒ��� ���� ��ġ ���
        Vector3 bottomPosition = originalPosition - transform.up * (originalScale.y / 4);

        // ���� ������Ʈ ����
        GameObject topObstacle = Instantiate(_middleObstacle, topPosition, originalRotation, transform.parent);
        topObstacle.transform.localScale = middleScale;

        // �Ʒ��� ������Ʈ ����
        GameObject bottomObstacle = Instantiate(_middleObstacle, bottomPosition, originalRotation, transform.parent);
        bottomObstacle.transform.localScale = middleScale;

        // ���� ������Ʈ �ı�
        Destroy(this.gameObject);
    }
}

