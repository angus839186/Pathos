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
        hearts = new List<HealthPoint>(GetComponentsInChildren<HealthPoint>());
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSceneLoaded += OnPlayerSpawned;
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSceneLoaded -= OnPlayerSpawned;
        }
        // 若 playerHealth 不為 null，也要記得解除訂閱
        if (playerHealth != null)
        {
            playerHealth.OnHealthUpdateEvent -= UpdateHealth;
        }
    }
    private void OnPlayerSpawned()
    {
        Debug.Log("Player spawned!");
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnHealthUpdateEvent += UpdateHealth;
            UpdateHealth(playerHealth.health);
        }
        else
        {
            Debug.LogWarning("PlayerHealth not found after player spawned!");
        }
    }

    public void UpdateHealth(int health)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i >= health)
            {
                hearts[i].Break();
            }
        }
    }
}
