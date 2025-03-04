using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager Instance;

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

    public void OnClickStartButton()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadGameScene("testScene"));
        DataPersistenceManager.Instance.NewGame();
    }
    public void OnClickContinueButton(CanvasGroup canvas)
    {
        MenuTransition transitionMenu = FindObjectOfType<MenuTransition>();
        transitionMenu.Transition(canvas);
        PlayerInputManager.Instance.SwitchActionMap("SaveMenu");
    }
    public void OnClickSettingButton()
    {

    }
    public void OnCloseButton()
    {
        Application.Quit();
    }

    public void OpenSaveMenu()
    {
        CanvasGroup saveMenu = FindObjectOfType<SaveFileManager>().GetComponent<CanvasGroup>();
        saveMenu.alpha = 1f;
        saveMenu.interactable = true;
        saveMenu.blocksRaycasts = true;
        SaveFileManager.Instance.OnOpenSaveFilePage();
    }
    public void CloseSaveMenu()
    {
        CanvasGroup saveMenu = FindObjectOfType<SaveFileManager>().GetComponent<CanvasGroup>();
        saveMenu.alpha = 0f;
        saveMenu.interactable = false;
        saveMenu.blocksRaycasts = false;
    }
}
