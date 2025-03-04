using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveFileMenu : Menu
{

    public static SaveFileMenu Instance;

    public TextMeshProUGUI saveFileIndexText;  // 顯示存檔欄位資訊
    public TextMeshProUGUI sceneNameText;      // 顯示存檔的場景名稱
    public TextMeshProUGUI gameTimeText;       // 顯示遊玩時間

    public Button loadButton;
    public Button saveButton;

    public Button deleteButton;

    public Button returnButton;

    public List<SceneSpriteMapping> sceneSpriteList;

    private SaveSlot[] saveSlots;

    private SaveSlot selectedSaveSlot;

    // 預設圖片（若找不到對應場景時使用）
    public Sprite defaultSceneSprite;

    // 建立字典以方便查詢
    private Dictionary<string, Sprite> sceneSprites;

    public CanvasGroup saveFileCanvas;

    [Header("select save file and scroll")]
    public List<RectTransform> buttons;
    public float animationSpeed;

    public RectTransform LoadDataCenter;
    public Vector2 centerOffset;
    public Vector2 upOffset;
    public Vector2 downOffset;

    public float maxDistance = 200f;

    public int selectedIndex = 0;


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

        saveSlots = this.GetComponentsInChildren<SaveSlot>();
        saveFileCanvas = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        IntializeSaveFile();
        UpdateButtonPositions();
    }

    public void IntializeSaveFile()
    {
        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }
    }

    public void OnBackButtonClicked()
    {
        Transition(mainMenuCanva);
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        DisplaySaveFileDetail(saveSlot);
    }

    public void DisplaySaveFileDetail(SaveSlot saveSlot)
    {
        if (saveSlot == null) return;

        saveFileIndexText.text = saveSlot.GetProfileId();
        sceneNameText.text = saveSlot.sceneName;
        gameTimeText.text = saveSlot.gameTime;
    }

    public void OnLoadFileButtonClicked()
    {
        DataPersistenceManager.Instance.LoadGame();
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadGameScene(DataPersistenceManager.Instance.gameData.currentScene));
    }

    public void DisableAllButtons()
    {

    }
    public void ActivateSaveFileCanvas()
    {
        saveFileCanvas.alpha = 1;
        saveFileCanvas.blocksRaycasts = true;
        saveFileCanvas.interactable = true;
    }

    public void DeactiveSaveFileCanvas()
    {
        saveFileCanvas.alpha = 0;
        saveFileCanvas.blocksRaycasts = false;
        saveFileCanvas.interactable = false;
    }
    public void SelectButton(int index)
    {
        if (index < 0 || index >= buttons.Count) return;

        selectedIndex = index;
        UpdateButtonPositions();
    }

    public void UpdateButtonPositions()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            RectTransform button = buttons[i];
            Vector2 targetPosition;

            if (i < selectedIndex)
            {
                int offsetCount = selectedIndex - i;
                targetPosition = centerOffset + upOffset * offsetCount;
            }
            else if (i > selectedIndex)
            {
                int offsetCount = i - selectedIndex;
                targetPosition = centerOffset + downOffset * offsetCount;
            }
            else
            {
                targetPosition = centerOffset;
            }


            StartCoroutine(SmoothMove(button, targetPosition));
        }
    }

    private IEnumerator SmoothMove(RectTransform button, Vector2 targetPosition)
    {
        // 如果 button 一開始就不存在，直接退出
        if (button == null) yield break;

        Vector2 startPosition = button.anchoredPosition;
        float elapsedTime = 0f;

        CanvasGroup cg = button.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = button.gameObject.AddComponent<CanvasGroup>();
        }

        float startAlpha = cg.alpha;
        float distance = Vector2.Distance(targetPosition, centerOffset);
        float targetAlpha = Mathf.Clamp01(1 - (distance / maxDistance));

        while (elapsedTime < animationSpeed)
        {
            // 每次更新前檢查
            if (button == null) yield break;

            float t = elapsedTime / animationSpeed;
            button.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最後再次確認 button 是否存在再更新
        if (button != null)
        {
            button.anchoredPosition = targetPosition;
            cg.alpha = targetAlpha;
        }
    }
}
[System.Serializable]
public struct SceneSpriteMapping
{
    public string sceneName;
    public Sprite sceneSprite;
}
