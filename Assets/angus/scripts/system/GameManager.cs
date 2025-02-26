using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string startScene = "testScene";
    public GameObject playerPrefab;

    public bool SetPlayer;

    public event Action OnPlayerSpawned;

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

    void OnEnable()
    {
        OnPlayerSpawned += SceneLoadData;
    }

    void OnDisable()
    {
        OnPlayerSpawned -= SceneLoadData;
    }

    public IEnumerator LoadGameScene(string sceneName)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;
        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        if (cam != null)
        {
            cam.Follow = player.transform;
        }
        SetPlayer = true;
        OnPlayerSpawned?.Invoke();
    }
    public void SceneLoadData()
    {
        DataPersistenceManager.Instance.LoadGameData();
    }
}


