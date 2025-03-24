using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public void OnStartButtonClicked()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.LoadLevel(gameManager.defaultScene);
        DataPersistenceManager.Instance.NewGame();
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
