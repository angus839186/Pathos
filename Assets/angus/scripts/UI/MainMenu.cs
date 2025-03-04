using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public void OnStartButtonClicked()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadGameScene(gameManager.defaultScene));
        DataPersistenceManager.Instance.NewGame();
    }
    public void OnContinueButtonClicked()
    {
        Transition(saveFileCanva);
        PlayerInputManager.Instance.SwitchActionMap("SaveMenu");
        SaveFileMenu.Instance.SelectButton(0);
    }
    public void OnSettingButtonClicked()
    {

    }
    public void OnLeaveButtonClicked()
    {

    }
}
