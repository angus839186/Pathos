using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int health = 3;
    void OnCollisionEnter2D(Collision2D collision)
    {
        FireBall fireBall = collision.gameObject.GetComponent<FireBall>();
        if(fireBall != null)
        {
            health--;
            UpdateHealthUI();
        }
    }
    void UpdateHealthUI()
    {
        Debug.Log("Player Health: " + health);
    }
}
