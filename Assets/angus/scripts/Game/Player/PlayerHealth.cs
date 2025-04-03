using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    private int maxHealth = 3;
    private Animator anime;

    public AudioClip heartBreakSound;

    public Action<int> OnHealthUpdateEvent;

    void Start()
    {
        anime = GetComponent<Animator>();
    }
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.H))
        // {
        //     TakeDamage(1);
        // }
    }

    public void TakeDamage(int damage)
    {
        if(health <= 0)return;
        anime.SetTrigger("hurt");
        AudioManager.instance.PlaySound(heartBreakSound);
        int currentHealth = health;
        currentHealth -= damage;
        UpdateHealth(currentHealth);
        PlayerController player = GetComponent<PlayerController>();
        player.ToggleMove(true);
        PlayerInteraction playerInteract = GetComponent<PlayerInteraction>();
        playerInteract.isInteracting = false;
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
        deathUI death = FindObjectOfType<deathUI>();
        death.PlayerDeath();
        PlayerInputManager.Instance.SwitchActionMap("Die");
    }
    public void Recover()
    {
        int currentHealth = health;
        currentHealth = maxHealth;
        UpdateHealth(currentHealth);
        PlayerInteraction playerInteract = GetComponent<PlayerInteraction>();
        playerInteract.isInteracting = false;
        PlayerController player = GetComponent<PlayerController>();
        player.ToggleMove(true);
    }
}
