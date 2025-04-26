using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public static MainMenu Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        canvasGroup = this.GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
    }
    public void OnStartButtonClicked()
    {
        ToggleCanvasGroup(false);
        GameStartPreview newGamePreview = FindObjectOfType<GameStartPreview>();
        if(newGamePreview.played)
        {
            newGamePreview.StartGame();
        }
        else
        {
            VideoController.Instance.GetVideo(newGamePreview.clip, newGamePreview.pausePoints);
        }
    }
    public void OnContinueButtonClicked()
    {
        Transition(saveFileCanva);
        SaveFileMenu.Instance.OnSaveSlotClicked(SaveFileMenu.Instance.saveSlots[SaveFileMenu.Instance.selectedIndex]);
        PlayerInputManager.Instance.SwitchActionMap("SaveMenu");
    }
    public void OnSettingButtonClicked()
    {
        Transition(settingCanva);
        PlayerInputManager.Instance.SwitchActionMap("SettingMenu");
    }
    public void OnLeaveButtonClicked()
    {
        Application.Quit();
    }
}
