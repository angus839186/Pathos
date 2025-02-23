using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public string startScene = "testScene";
    public GameObject playerPrefab;

    public bool SetPlayer;

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
    private void Start()
    {
        Debug.Log("Start");
        if (SceneManager.GetActiveScene().name == startScene)
        {
            SpawnPlayer();
        }
    }

    public IEnumerator LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(startScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SpawnPlayer();
    }

    // 尋找生成點並根據位置實例化角色
    private void SpawnPlayer()
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;
        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        cam.Follow = player.transform;
    }
}
