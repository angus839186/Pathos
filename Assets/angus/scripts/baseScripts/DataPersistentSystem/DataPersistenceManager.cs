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

    public string selectedProfileId = "";
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

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    // private void OnEnable()
    // {
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // private void OnDisable()
    // {
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

    // public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     LoadGame();
    // }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        // update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
    }

    public void DeleteProfileData(string profileId)
    {
        // delete the data for this profile id
        dataHandler.Delete(profileId);
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void SaveGame()
    {
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }
        SaveGameData();
        dataHandler.Save(gameData, selectedProfileId);
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load(selectedProfileId);
    }

    public void LoadGameData()
    {
        if (this.gameData == null)
        {
            return;
        }

        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
            // MonoBehaviour monoBehaviour = dataPersistenceObj as MonoBehaviour;
            // if (monoBehaviour != null)
            // {
            //     Debug.Log("找到的物件名稱: " + monoBehaviour.name);
            // }
        }
    }
    public void SaveGameData()
    {
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveData(ref gameData);
            // MonoBehaviour monoBehaviour = dataPersistence as MonoBehaviour;
            // if (monoBehaviour != null)
            // {
            //     Debug.Log("找到的物件名稱: " + monoBehaviour.name);
            // }
        }
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
