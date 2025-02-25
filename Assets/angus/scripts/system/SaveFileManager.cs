using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveFileManager : MonoBehaviour
{

    public static SaveFileManager Instance;
    public GameObject buttonPrefab;      // 在 Inspector 指定按鈕預製件
    public SaveFileChecker saveFileChecker; // 引用 SaveFileChecker 腳本

    public TextMeshProUGUI saveFileIndexText;  // 顯示存檔欄位資訊
    public TextMeshProUGUI sceneNameText;      // 顯示存檔的場景名稱
    public TextMeshProUGUI gameTimeText;       // 顯示遊玩時間

    // 當前被選中的存檔欄位
    private SaveFileInfo selectedSaveFile;

    public Button loadButton;
    public Button saveButton;


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
        // 確保 SaveFileChecker 已初始化
        if (saveFileChecker == null)
        {
            saveFileChecker = FindObjectOfType<SaveFileChecker>();
        }
    }
    void OnEnable()
    {
        SceneDataManager.Instance.OnSceneLoad += GetAllSaveFiles;
    }
    void OnDisable()
    {
        SceneDataManager.Instance.OnSceneLoad -= GetAllSaveFiles;
    }

    public void GetAllSaveFiles()
    {
        saveFileIndexText = GameObject.Find("存檔欄位").GetComponent<TextMeshProUGUI>();
        sceneNameText = GameObject.Find("場景名稱").GetComponent<TextMeshProUGUI>();
        gameTimeText = GameObject.Find("遊玩時間").GetComponent<TextMeshProUGUI>();
        loadButton = GameObject.Find("讀檔按鈕").GetComponent<Button>();
        saveButton = GameObject.Find("存檔按鈕").GetComponent<Button>();
        loadButton.onClick.AddListener(() => OnLoadButtonClicked());
        saveButton.onClick.AddListener(() => OnSaveButtonClicked());
        foreach (SaveFileInfo saveFile in saveFileChecker.saveFiles)
        {
            // 動態生成按鈕
            ButtonScrollController scrollController = FindObjectOfType<ButtonScrollController>();
            GameObject newSaveButton = Instantiate(buttonPrefab, scrollController.transform);
            Button btn = newSaveButton.GetComponent<Button>();

            RectTransform newButtonRect = newSaveButton.GetComponent<RectTransform>();

            // 為按鈕添加點擊事件，當點擊時更新右側資訊
            btn.onClick.AddListener(() => OnSlotButtonClicked(saveFile));

            // 將新按鈕加入控制器的列表
            if (scrollController != null)
            {
                scrollController.buttons.Add(newButtonRect);

                // 取得目前按鈕在列表中的索引
                int index = scrollController.buttons.Count - 1;
                // 當按鈕被點擊時，呼叫 SelectButton 並傳入該索引
                btn.onClick.AddListener(() => scrollController.SelectButton(index));
            }
            scrollController.centerOffset = new Vector2(scrollController.LoadDataCenter.anchoredPosition.x,
             scrollController.LoadDataCenter.anchoredPosition.y);
            scrollController.UpdateButtonPositions();
        }
    }

    // 當點擊存檔欄位按鈕時更新資訊
    void OnSlotButtonClicked(SaveFileInfo saveFile)
    {
        selectedSaveFile = saveFile;
        // 更新欄位編號資訊
        saveFileIndexText.text = "存檔欄位: " + saveFile.fileName;

        // 若該欄位有存檔資料則顯示詳細資訊，否則提示為空
        if (saveFile.data != null)
        {
            sceneNameText.text = "場景: " + saveFile.data.currentScene;
            gameTimeText.text = "遊玩時間: " + saveFile.data.gameTime.ToString("F2") + "秒";
            loadButton.gameObject.SetActive(true);
            saveButton.gameObject.SetActive(false);
        }
        else
        {
            sceneNameText.text = "場景: 空";
            gameTimeText.text = "遊玩時間: 0秒";
            loadButton.gameObject.SetActive(false);
            saveButton.gameObject.SetActive(true);
        }
    }

    // 點擊【讀檔】按鈕時呼叫
    public void OnLoadButtonClicked()
    {
        if (selectedSaveFile != null && selectedSaveFile.data != null)
        {
            // 使用 DataPersistenceManager 載入該欄位的存檔
            DataPersistenceManager.Instance.gameData = selectedSaveFile.data;

            // 讀取存檔中的場景資訊並切換
            if (!string.IsNullOrEmpty(DataPersistenceManager.Instance.gameData.currentScene))
            {
                GameManager.Instance.StartCoroutine(GameManager.Instance.
                LoadGameScene(DataPersistenceManager.Instance.gameData.currentScene));
            }
        }
        else
        {
            Debug.Log("該欄位沒有存檔，無法讀取！");
        }
    }

    // 點擊【存檔】按鈕時呼叫
    public void OnSaveButtonClicked()
    {
        if (SceneManager.GetActiveScene().name == "testMenu")
        {
            Debug.LogError("你還沒開始遊戲");
            return;
        }
        if (selectedSaveFile != null)
        {
            // 將 DataPersistenceManager 的 fileName 設定為選定的欄位
            DataPersistenceManager.Instance.fileName = selectedSaveFile.fileName;
            DataPersistenceManager.Instance.SaveGame();

            // 更新 SaveFileChecker 與右側資訊（依照需求可以重新讀取最新存檔資料）
            Debug.Log("存檔成功於: " + selectedSaveFile.fileName);
        }
        else
        {
            Debug.Log("請先選擇一個存檔欄位！");
        }
    }
}
