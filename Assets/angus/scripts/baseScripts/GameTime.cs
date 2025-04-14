using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameTime : MonoBehaviour, IDataPersistence
{
    public static GameTime Instance;
    public bool playing;
    public float elapsedTime = 0f;

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

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().name == GameManager.Instance.menuScene)
        {
            playing = false;
            elapsedTime = 0f;
        }
        else
        {
            playing = true;
        }
    }

    public void LoadData(GameData data)
    {
        elapsedTime = data.gameTime;
    }

    public void SaveData(ref GameData data)
    {
        data.gameTime = elapsedTime;
    }

    void Update()
    {
        if(playing)
        {
            elapsedTime += Time.deltaTime;
        }
    }
}
