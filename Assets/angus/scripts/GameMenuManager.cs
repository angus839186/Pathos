using UnityEngine;
using System;

public class GameMenuManager : MonoBehaviour
{
    public void OnClickStartButton()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(gameManager.LoadGameScene());
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

    }
    public void FadeOutMenu()
    {

    }
    public void FadeInMenu()
    {

    }
}
