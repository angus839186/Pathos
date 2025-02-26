using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerInitializer : MonoBehaviour
{
    public GameObject backpackUIManagerPrefab;

    void Awake()
    {
        // 只在遊戲關卡中執行初始化
        if (SceneManager.GetActiveScene().name != "testMenu")
        {
            if (BackpackUIManager.Instance == null && backpackUIManagerPrefab != null)
            {
                Instantiate(backpackUIManagerPrefab);
            }
        }
    }
}
