using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager Instance;
    public CanvasGroup saveMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        MenuInputManager.Instance.OnCloseSaveMenuEvent += CloseSaveMenu;
        SceneDataManager.Instance.OnSceneLoad += GetSaveMenu;
    }

    void OnDisable()
    {
        MenuInputManager.Instance.OnCloseSaveMenuEvent -= CloseSaveMenu;
        SceneDataManager.Instance.OnSceneLoad -= GetSaveMenu;

    }
    public void OnClickStartButton()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadGameScene("testScene"));
        DataPersistenceManager.Instance.NewGame();
    }
    public void OnClickLoadButton(CanvasGroup canvas)
    {
        MenuTransition.Instance.Transition(canvas);
    }
    public void OnClickSettingButton()
    {

    }
    public void OnCloseButton()
    {
        Application.Quit();
    }

    public void GetSaveMenu()
    {
        CanvasGroup canva = GameObject.Find("存讀檔頁面UI").GetComponent<CanvasGroup>();
        saveMenu = canva;
    }

    public void OpenSaveMenu()
    {
        saveMenu.alpha = 1f;
        MenuInputManager.Instance.SwitchActionMap("SaveMenu");
        saveMenu.interactable = true;
        saveMenu.blocksRaycasts = true;
        SaveFileManager.Instance.OnOpenSaveFilePage();
    }
    public void CloseSaveMenu()
    {
        saveMenu.alpha = 0f;
        MenuInputManager.Instance.SwitchActionMap("MainMenu");
        saveMenu.interactable = false;
        saveMenu.blocksRaycasts = false;
    }
}
