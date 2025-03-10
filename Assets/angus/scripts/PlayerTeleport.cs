using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleport : MonoBehaviour {
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // 找到所有的重生點
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        foreach (SpawnPoint sp in spawnPoints) {
            if (sp.spawnID == SpawnManager.spawnPointID) {
                transform.position = sp.transform.position;
                break;
            }
        }
    }
}
