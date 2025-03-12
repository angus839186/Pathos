using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string defaultScene = "testGrass";
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

    public IEnumerator LoadGameScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if(!SetPlayer)
        {
            SpawnPlayer();
        }

        while(SetPlayer == false)
        {
            yield return null;
        }

        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (cam != null)
        {
            cam.Follow = player.transform;
        }
        DataPersistenceManager.Instance.LoadGameData();

        PlayerInputManager.Instance.SwitchActionMap("Player");
    }

    private void SpawnPlayer()
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;
        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        SetPlayer = true;
    }
}


