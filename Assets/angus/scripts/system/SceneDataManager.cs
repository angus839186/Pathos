using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataManager : MonoBehaviour, IDataPersistence
{
    public static SceneDataManager Instance;
    public string currentScene;

    public string defaultScene;

    public event Action OnSceneLoad;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoad.Invoke();
    }
    public void LoadData(GameData data)
    {
        //Do Nothing
    }

    public void SaveData(ref GameData data)
    {
        data.currentScene = SceneManager.GetActiveScene().name;
    }
}
