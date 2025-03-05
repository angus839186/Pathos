using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public void OnStartButtonClicked()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadGameScene(gameManager.defaultScene));
        DataPersistenceManager.Instance.LoadGame();
    }
    public void OnContinueButtonClicked()
    {
        Transition(saveFileCanva);
        PlayerInputManager.Instance.SwitchActionMap("SaveMenu");
    }
    public void OnSettingButtonClicked()
    {

    }
    public void OnLeaveButtonClicked()
    {

    }
}
