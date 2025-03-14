using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceCamera : MonoBehaviour
{
    public static PersistenceCamera Instance;
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
}
