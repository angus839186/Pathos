using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance;
    public List<HealthPoint> hearts;

    public PlayerHealth playerHealth;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDisable()
    {

    }

    void Start()
    {

    }

    public void UpdateHealth(int health)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < health)
            {
                return;
            }
            else
            {
                hearts[i].Break();
            }
        }
    }
}
