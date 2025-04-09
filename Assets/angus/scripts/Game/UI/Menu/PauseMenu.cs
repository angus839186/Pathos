using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    public CanvasGroup pauseMenuCanvas;

    public PlayerTutorial playerTutorial;
    void Awake()
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
        PlayerInputManager.Instance.OnTogglePauseMenuEvent += TogglePauseMenu;
    }
    void OnDisable()
    {
        PlayerInputManager.Instance.OnTogglePauseMenuEvent -= TogglePauseMenu;
    }

    public void TogglePauseMenu(bool toggle)
    {
        if(toggle == false)
        {
            PlayerInputManager.Instance.SwitchActionMap("Player");
        }
        else
        {
            PlayerInputManager.Instance.SwitchActionMap("PauseMenu");
        }
        Time.timeScale = toggle ? 0f : 1f;
        pauseMenuCanvas.alpha = toggle ? 1f : 0f;
        pauseMenuCanvas.interactable = toggle;
        pauseMenuCanvas.blocksRaycasts = toggle;
    }
    public void OnContinueButtonClicked()
    {
        TogglePauseMenu(false);
    }
    public void OnSettingButtonClicked()
    {
        SettingMenu.Instance.ToggleSettingMenu(true);
    }

    public void OnPlayTutorialButtonClicked()
    {
        playerTutorial.TogglePlayerTutorialCanva(true);
    }

    public void OnQuitGameButtonClicked()
    {
        TogglePauseMenu(false);
        GameManager.Instance.BackToMenu();
    }
}
