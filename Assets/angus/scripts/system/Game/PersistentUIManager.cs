using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentUIManager : MonoBehaviour
{
    public static PersistentUIManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 讓整個 UI 管理器物件在切換場景時不被銷毀
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
