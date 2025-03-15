using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;
using System;

public class GameManager : MonoBehaviour
{

    public string defaultScene = "testGrass";
    public static GameManager Instance;
    public GameObject playerPrefab;

    public GameObject cameraPrefab;

    public bool SetPlayer;

    public bool SetCamera;

    public Action OnSceneLoaded;

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
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadGameLevel(levelName));
    }
    IEnumerator LoadGameLevel(string sceneName)
    {
        if (!IsSceneLoaded("HealthUI"))
        {
            SceneManager.LoadScene("HealthUI", LoadSceneMode.Additive);
        }
        while (!IsSceneLoaded("HealthUI"))
        {
            yield return null;
        }

        if (!SetPlayer)
        {
            SpawnPlayer();
        }

        while (SetPlayer == false)
        {
            yield return null;
        }

        if(!SetCamera)
        {
            SpawnCamera();
        }

        while(SetCamera == false)
        {
            yield return null;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;
            player.transform.position = spawnPos;
        }
        DataPersistenceManager.Instance.LoadGameData();

        PlayerInputManager.Instance.SwitchActionMap("Player");
        OnSceneLoaded?.Invoke();
    }
    public IEnumerator LoadNextScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        PlayerController player = FindAnyObjectByType<PlayerController>();
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        if (cam != null)
        {
            cam.Follow = player.transform;
        }
        DataPersistenceManager.Instance.LoadGameData();

        PlayerInputManager.Instance.SwitchActionMap("Player");
        OnSceneLoaded?.Invoke();
    }

    public void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        SetPlayer = true;
        Debug.Log("Player spawned");
    }

    public void SpawnCamera()
    {
        GameObject camera = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
        SetCamera = true;
        Debug.Log("Camera spawned");
    }
    bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
                return true;
        }
        return false;
    }
}


