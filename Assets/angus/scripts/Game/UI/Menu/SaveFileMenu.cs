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

    public SaveSlot currentSaveSlot;

    // 建立字典以方便查詢
    public Dictionary<string, Sprite> sceneSprites;

    public CanvasGroup saveFileCanvas;

    [Header("select save file and scroll")]
    public List<RectTransform> buttons;
    public float animationSpeed;
    public Vector2 centerOffset;
    public Vector2 upOffset;
    public Vector2 downOffset;

    public float maxDistance = 200f;

    public int selectedIndex = 0;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        loadButton.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);
        UpdateSaveFile();
        SelectButton(selectedIndex);
    }

    public void UpdateSaveFile()
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

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        currentSaveSlot = saveSlot;

        selectedIndex = currentSaveSlot.index;

        DataPersistenceManager.Instance.ChangeSelectedProfileId(currentSaveSlot.GetProfileId());

        DisplaySaveFileDetail(currentSaveSlot);

        SelectButton(selectedIndex);
    }

    public void DisplaySaveFileDetail(SaveSlot saveSlot)
    {
        loadButton.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);

        saveFileIndexText.text = saveSlot.GetProfileId();
        sceneNameText.text = saveSlot.sceneName;
        gameTimeText.text = saveSlot.gameTime;

        if (SceneManager.GetActiveScene().name == GameManager.Instance.menuScene)
        {
            if (saveSlot.gameData == null)
            {
                returnButton.gameObject.SetActive(true);
            }
            else
            {
                loadButton.gameObject.SetActive(true);
                deleteButton.gameObject.SetActive(true);
                returnButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (saveSlot.gameData == null)
            {
                saveButton.gameObject.SetActive(true);
                returnButton.gameObject.SetActive(true);
            }
            else
            {
                saveButton.gameObject.SetActive(true);
                deleteButton.gameObject.SetActive(true);
                returnButton.gameObject.SetActive(true);
            }
        }

    }

    public void ActivateSaveFileCanvas()
    {
        saveFileCanvas.alpha = 1;
        saveFileCanvas.blocksRaycasts = true;
        saveFileCanvas.interactable = true;
        SelectButton(selectedIndex);
    }

    public void DeactiveSaveFileCanvas()
    {
        saveFileCanvas.alpha = 0;
        saveFileCanvas.blocksRaycasts = false;
        saveFileCanvas.interactable = false;
    }

    #region savefileButtonFunction

    public void OnLoadFileButtonClicked()
    {
        DataPersistenceManager.Instance.LoadGame();
        GameManager gameManager = GameManager.Instance;
        gameManager.LoadLevel(DataPersistenceManager.Instance.gameData.currentScene);
        DeactiveSaveFileCanvas();
    }

    public void OnDeleteButtonClicked()
    {
        DataPersistenceManager.Instance.DeleteProfileData(currentSaveSlot.GetProfileId());

        UpdateSaveFile();

        DisplaySaveFileDetail(currentSaveSlot);
    }

    public void OnSaveButtonClicked()
    {
        DataPersistenceManager.Instance.SaveGame();
        UpdateSaveFile();

        DisplaySaveFileDetail(currentSaveSlot);
    }
    public void OnBackButtonClicked()
    {
        if (SceneManager.GetActiveScene().name == GameManager.Instance.menuScene)
        {
            Transition(mainMenuCanva);
            PlayerInputManager.Instance.SwitchActionMap("MainMenu");

        }
        else
        {
            DeactiveSaveFileCanvas();
        }
    }
    #endregion


    #region savefileScroll
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
    #endregion
}
[System.Serializable]
public struct SceneSpriteMapping
{
    public string sceneName;
    public Sprite sceneSprite;
}
