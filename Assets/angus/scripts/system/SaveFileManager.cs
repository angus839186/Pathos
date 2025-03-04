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

    public Button deleteButton;

    public Button returnButton;

    public List<SceneSpriteMapping> sceneSpriteList;

    // 預設圖片（若找不到對應場景時使用）
    public Sprite defaultSceneSprite;

    // 建立字典以方便查詢
    private Dictionary<string, Sprite> sceneSprites;

    public CanvasGroup saveFileCanvas;


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
        sceneSprites = new Dictionary<string, Sprite>();
        foreach (var mapping in sceneSpriteList)
        {
            if (!sceneSprites.ContainsKey(mapping.sceneName))
            {
                sceneSprites.Add(mapping.sceneName, mapping.sceneSprite);
            }
        }
    }
    void Start()
    {
        if (saveFileChecker == null)
        {
            saveFileChecker = FindObjectOfType<SaveFileChecker>();
        }
        saveFileCanvas = GetComponent<CanvasGroup>();
        saveFileCanvas.alpha = 0f;
        saveFileCanvas.interactable = false;
        saveFileCanvas.blocksRaycasts = false;
    }
    void OnEnable()
    {
        PlayerInputManager.Instance.OnCloseSaveMenuEvent += OnReturnMenuButtonClick;
        SceneDataManager.Instance.OnSceneLoad += GetAllSaveFiles;
        SceneDataManager.Instance.OnSceneLoad += AddSaveFileMenuButton;
    }
    void OnDisable()
    {
        PlayerInputManager.Instance.OnCloseSaveMenuEvent -= OnReturnMenuButtonClick;
        SceneDataManager.Instance.OnSceneLoad -= GetAllSaveFiles;
        SceneDataManager.Instance.OnSceneLoad -= AddSaveFileMenuButton;

    }

    public void AddSaveFileMenuButton()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        returnButton.onClick.RemoveAllListeners();


        if (currentSceneName == "testMenu")
        {
            returnButton.onClick.AddListener(() => OnReturnMenuButtonClick());
        }
        else
        {
            returnButton.onClick.AddListener(() => OnReturnGameSceneButtonClicked());
        }
    }

    public void GetAllSaveFiles()
    {
        ButtonScrollController scrollController = FindObjectOfType<ButtonScrollController>();

        // 清除 Scroll Container 中先前建立的所有按鈕
        foreach (Transform child in scrollController.transform)
        {
            Destroy(child.gameObject);
        }

        scrollController.buttons.Clear();

        // 重新產生存檔欄位的按鈕
        foreach (SaveFileInfo saveFile in saveFileChecker.saveFiles)
        {
            GameObject newSaveButton = Instantiate(buttonPrefab, scrollController.transform);
            Button btn = newSaveButton.GetComponent<Button>();

            // 取得按鈕上的 Image 元件 (假設按鈕預製件下有一個 Image)
            Image[] sceneImage = newSaveButton.GetComponentsInChildren<Image>();
            if (saveFile.data != null)
            {
                if (sceneSprites.ContainsKey(saveFile.data.currentScene))
                {
                    sceneImage[1].sprite = sceneSprites[saveFile.data.currentScene];
                }
                else
                {
                    sceneImage[1].sprite = defaultSceneSprite;
                }
            }
            else
            {
                sceneImage[1].sprite = defaultSceneSprite;
            }

            // 為按鈕添加點擊事件
            btn.onClick.AddListener(() => OnSlotButtonClicked(saveFile));

            // 將新按鈕加入控制器的列表
            scrollController.buttons.Add(newSaveButton.GetComponent<RectTransform>());
            int index = scrollController.buttons.Count - 1;
            btn.onClick.AddListener(() => scrollController.SelectButton(index));

            scrollController.centerOffset = new Vector2(
                scrollController.LoadDataCenter.anchoredPosition.x,
                scrollController.LoadDataCenter.anchoredPosition.y);
            scrollController.UpdateButtonPositions();
        }
    }

    private void UpdateSlotUI(SaveFileInfo saveFile)
    {
        // 先將所有按鈕隱藏
        loadButton.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);

        // 取得當前場景名稱
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (saveFile.data != null)
        {
            // 如果該欄位有存檔資料
            if (currentSceneName == "testMenu")
            {
                // 主菜單：顯示讀檔與刪除按鈕
                loadButton.gameObject.SetActive(true);
                deleteButton.gameObject.SetActive(true);
            }
            else
            {
                // 遊戲進行中：顯示覆蓋存檔（存檔按鈕）與刪除按鈕
                saveButton.gameObject.SetActive(true); // 當作覆蓋存檔使用
                returnButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (currentSceneName == "testMenu")
            {
                returnButton.gameObject.SetActive(true);
                return;
            }
            else
            {
                saveButton.gameObject.SetActive(true);
                returnButton.gameObject.SetActive(true);
            }
        }
    }

    void OnSlotButtonClicked(SaveFileInfo saveFile)
    {
        selectedSaveFile = saveFile;
        // 更新欄位資訊
        saveFileIndexText.text = "存檔欄位: " + saveFile.fileName;

        // 更新右側資訊文字
        if (saveFile.data != null)
        {
            sceneNameText.text = "場景: " + saveFile.data.currentScene;
            gameTimeText.text = "遊玩時間: " + saveFile.data.gameTime.ToString("F2") + "秒";
        }
        else
        {
            sceneNameText.text = "場景: 空";
            gameTimeText.text = "遊玩時間: 0秒";
        }

        // 根據當前場景及欄位狀態更新按鈕顯示
        UpdateSlotUI(saveFile);
    }

    public void OnOpenSaveFilePage()
    {
        loadButton.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);
    }

    // 點擊【讀檔】按鈕時呼叫
    public void OnLoadButtonClicked()
    {
        if (selectedSaveFile != null && selectedSaveFile.data != null)
        {
            DataPersistenceManager.Instance.gameData = selectedSaveFile.data;
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
            // 設定存檔欄位檔名
            DataPersistenceManager.Instance.fileName = selectedSaveFile.fileName;
            DataPersistenceManager.Instance.SaveGame();
            Debug.Log("存檔成功於: " + selectedSaveFile.fileName);

            // 更新該存檔欄位的資料（可以重新讀取或直接指定）
            // 例如：selectedSaveFile.data = DataPersistenceManager.Instance.gameData;
            // 接著刷新 UI
            UpdateSlotUI(selectedSaveFile);
        }
        else
        {
            Debug.Log("請先選擇一個存檔欄位！");
        }
    }
    public void OnDeleteButtonClicked()
    {
        if (selectedSaveFile != null)
        {
            // 建立 FileDataHandler 並刪除檔案
            FileDataHandler fileHandler = new FileDataHandler(Application.persistentDataPath, selectedSaveFile.fileName);
            fileHandler.DeleteSaveFile();

            // 清除存檔資料
            selectedSaveFile.data = null;

            // 刷新 SaveFileChecker 的存檔列表（假設你有提供此功能）
            SaveFileChecker.Instance.GetSave();

            // 更新 UI，使該欄位變為空
            UpdateSlotUI(selectedSaveFile);
            Debug.Log("存檔已刪除: " + selectedSaveFile.fileName);
        }
    }
    public void OnReturnMenuButtonClick()
    {
        MenuTransition transitionMenu = FindObjectOfType<MenuTransition>();
        CanvasGroup mainCanva = transitionMenu.mainMenuCanva;
        transitionMenu.Transition(mainCanva);
        PlayerInputManager.Instance.SwitchActionMap("MainMenu");
    }
    public void OnReturnGameSceneButtonClicked()
    {
        GameMenuManager.Instance.CloseSaveMenu();
    }
}
[System.Serializable]
public struct SceneSpriteMapping
{
    public string sceneName;
    public Sprite sceneSprite;
}
