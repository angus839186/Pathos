using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    private int maxHealth = 3;
    private Animator anime;

    public Action<int> OnHealthUpdateEvent;

    void Start()
    {
        anime = GetComponent<Animator>();
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
        if(health <= 0)return;
        anime.SetTrigger("hurt");
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
