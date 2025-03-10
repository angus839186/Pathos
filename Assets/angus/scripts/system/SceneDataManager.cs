using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneDataManager : MonoBehaviour, IDataPersistence
{
    public static SceneDataManager Instance;

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
    public void LoadData(GameData data)
    {
        //Do Nothing
    }
    public void SaveData(ref GameData data)
    {
        data.currentScene = SceneManager.GetActiveScene().name;
    }
}