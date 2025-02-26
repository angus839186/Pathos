using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage config")]
    [SerializeField] public string fileName;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance;

    public GameData gameData;
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


    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveData(ref gameData);
        }

        if (gameData != null)
        {
            Debug.Log(gameData);
            this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            dataHandler.Save(gameData);
        }
    }
    // public void LoadGame()
    // {

    //     this.gameData = dataHandler.Load();
    //     if (this.gameData == null)
    //     {
    //         NewGame();
    //     }

    //     foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
    //     {
    //         dataPersistence.LoadData(gameData);
    //     }
    //     Debug.Log("Loaded");
    // }

    public void LoadGame(string fileName)
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.gameData = dataHandler.Load();
    }

    public void LoadGameData()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.LoadData(gameData);
        }
    }

    // private void OnApplicationQuit()
    // {
    //     SaveGame();
    // }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
