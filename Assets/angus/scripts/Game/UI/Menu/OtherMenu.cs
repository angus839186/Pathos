using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherMenu : Menu
{
    public static OtherMenu Instance;

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
    public void OnEnable()
    {
        if (PlayerInputManager.Instance != null)
        {
            PlayerInputManager.Instance.OnCloseOtherMenuEvent += CloseOtherMenu;
        }
    }
    public void OnDisable()
    {
        if (PlayerInputManager.Instance != null)
        {
            PlayerInputManager.Instance.OnCloseOtherMenuEvent -= CloseOtherMenu;
        }
    }
    public void CloseOtherMenu()
    {
        Transition(mainMenuCanva);
    }
}
