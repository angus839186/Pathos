using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public string defaultScene;
    public string menuScene;
    public GameObject playerPrefab;

    public GameObject cameraPrefab;

    public GameObject playerInstance;
    public GameObject cameraInstance;

    public bool SetPlayer;

    public bool SetCamera;

    public bool SetUI;

    public Action OnSceneLoaded;

    public bool reversePlainBossWin;

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
        if (!SetPlayer)
        {
            SpawnPlayer();
        }

        while (SetPlayer == false)
        {
            yield return null;
        }

        if (!SetCamera)
        {
            SpawnCamera();
        }
        while (SetCamera == false)
        {
            yield return null;
        }
        if (!SceneManager.GetSceneByName("playerUI").isLoaded)
        {
            SceneManager.LoadScene("playerUI", LoadSceneMode.Additive);
            // 若需要，可以在這邊設定一個旗標，表示 UI 已載入
            SetUI = true;
        }
        // 如果需要等待 playerUI 載入完成也可以加上類似以下等待邏輯
        while (!SceneManager.GetSceneByName("playerUI").isLoaded)
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
            player.GetComponent<PlayerHealth>().Recover();
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
    public IEnumerator ReloadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("Reload");
        PlayerController player = FindAnyObjectByType<PlayerController>();
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        if(player != null)
        {
            player.GetComponent<PlayerHealth>().Recover();
            cam.Follow = player.transform;
        }

        DataPersistenceManager.Instance.LoadGameData();

        PlayerInputManager.Instance.SwitchActionMap("Player");
        OnSceneLoaded?.Invoke();
    }

    public void SpawnPlayer()
    {
        if (playerInstance == null)
        {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            SetPlayer = true;
            Debug.Log("Player spawned");
            playerInstance = player;
        }
        else
        {
            playerInstance.SetActive(true);
            SetPlayer = true;
        }
    }

    public void SpawnCamera()
    {
        if (cameraInstance == null)
        {
            GameObject camera = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
            SetCamera = true;
            Debug.Log("Camera spawned");
            cameraInstance = camera;
        }
        else
        {
            cameraInstance.SetActive(true);
            SetCamera = true;
        }
    }
    public void BackToMenu()
    {
        playerInstance.SetActive(false);
        cameraInstance.SetActive(false);
        SetPlayer = false;
        SetCamera = false;
        SceneManager.LoadScene(menuScene);
        PlayerInputManager.Instance.SwitchActionMap("MainMenu");
    }
}


