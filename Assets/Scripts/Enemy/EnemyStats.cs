using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // --- SERIALIZED FIELDS --- //
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    // --- FIELDS --- //
    private HealthBar healthBar;

    // --- UNITY METHODS --- //
    private void Awake()
    {
        healthBar = GetComponent<HealthBar>();
    }

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
    }

    
    private void Update()
    {
        
    }

    // --- PUBLIC METHODS --- //
    public void TakeDamage(int damage)
    {
        health -= damage;

        healthBar.SetHealth(health);
    }
}
