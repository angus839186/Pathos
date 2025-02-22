using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public string startScene = "grass"; // 遊戲場景名稱
    public GameObject playerPrefab;             // 角色預製體

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadGameScene() {
        // 非同步載入遊戲場景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(startScene);
        while (!asyncLoad.isDone) {
            yield return null;
        }
        // 當場景載入完成後，尋找生成點
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;
        // 根據生成點位置實例化角色
        Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    }
}
