using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public void OnStartButtonClicked()
    {
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
        PlayerInputManager.Instance.SwitchActionMap("SaveMenu");
    }
    public void OnSettingButtonClicked()
    {
        Transition(settingCanva);
        PlayerInputManager.Instance.SwitchActionMap("SettingMenu");
    }
    public void OnLeaveButtonClicked()
    {

    }
}
