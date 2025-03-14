using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int health = 3;
    private int maxHealth = 3;

    public Action<int> OnHealthUpdateEvent;

    void Start()
    {
        health = maxHealth;
        UpdateHealth(health);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        int currentHealth = health;
        currentHealth -= damage;
        UpdateHealth(currentHealth);
    }

    public void UpdateHealth(int newHealth)
    {
        health = newHealth;
        if(health <= 0)
        {
            Die();
        }
        OnHealthUpdateEvent?.Invoke(health);
    }
    public void Die()
    {
        Debug.Log("Player died");
    }
}
