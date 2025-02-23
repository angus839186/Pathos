using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager Instance;

    private GameData gameData;
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Error");
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach(IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveData(ref gameData);
        }
        Debug.Log(gameData.playerPosition);
    }
    public void LoadGame()
    {
        if(this.gameData == null)
        {
            NewGame();
        }

        foreach(IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.LoadData(gameData);
        }
        Debug.Log("Loaded");
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>()
        .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }
}
