using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cart : MonoBehaviour
{
    [Header("Card Health")]
    public float MaxHealth = 100;
    public Slider HealthSlider;
    public float CurrentHealth => _currentHealth;
    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = MaxHealth;
        HealthSlider.maxValue = 1f;
        HealthSlider.value = _currentHealth / MaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            _currentHealth -= 10f;
            HealthSlider.value = _currentHealth / MaxHealth;
            if(_currentHealth <= 0)
            {
                GameManager.Instance.MoveDefeatScene();
            }         
        }
    }
}
