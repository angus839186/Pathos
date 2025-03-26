using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        PlayerInputManager.Instance.OnClosePlayerTutorialEvent += TogglePlayerTutorialCanva;
    }

    void Start()
    {
        TogglePlayerTutorialCanva(false);
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnClosePlayerTutorialEvent -= TogglePlayerTutorialCanva;
    }
    public CanvasGroup playerTutorialCanva;
    public void TogglePlayerTutorialCanva(bool toggle)
    {
        playerTutorialCanva.alpha = toggle? 1f: 0f;
        playerTutorialCanva.blocksRaycasts = toggle;
        playerTutorialCanva.interactable = toggle;

        if(toggle == true)
        {
            PlayerInputManager.Instance.SwitchActionMap("PlayerTutorial");
        }
        else
        {
            PlayerInputManager.Instance.SwitchActionMap("PauseMenu");
        }
    }
}
