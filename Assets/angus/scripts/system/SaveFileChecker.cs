using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileChecker : MonoBehaviour
{
    public static SaveFileChecker Instance;
    public List<SaveFileInfo> saveFiles = new List<SaveFileInfo>();
    public List<string> fileNames = new List<string>();  // 例如 ["Slot1", "Slot2", "Slot3", "Slot4", "Slot5"]

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

    void Start()
    {
        GetSave();
    }
    public void GetSave()
    {
        // 遍歷所有預定的存檔欄位
        foreach (string fileName in fileNames)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(fullPath))
            {
                // 使用 FileDataHandler 讀取存檔資料
                FileDataHandler handler = new FileDataHandler(Application.persistentDataPath, fileName);
                GameData loadedData = handler.Load();
                Debug.Log("存檔存在：" + fullPath);
                saveFiles.Add(new SaveFileInfo() { fileName = fileName, data = loadedData });
            }
            else
            {
                Debug.Log("存檔不存在：" + fullPath);
                // 以空資料建立一個存檔欄位
                saveFiles.Add(new SaveFileInfo() { fileName = fileName, data = null });
            }
        }
        SaveFileManager.Instance.GetAllSaveFiles();
    }
}

public class SaveFileInfo
{
    public string fileName;
    public GameData data;
}
