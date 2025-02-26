using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    public static MenuInputManager Instance;

    public event Action BackToMainPanel;

    public event Action OnCloseSaveMenuEvent;

    public PlayerInput menuInput;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        menuInput.GetComponent<PlayerInput>();
    }
    public void OnClosePanel(InputValue value)
    {
        float pressed = value.Get<float>();
        if(pressed > 0.5f)
        {
            BackToMainPanel.Invoke();
        }
    }
    public void OnCloseSaveMenu(InputValue value)
    {
        float pressed = value.Get<float>();
        if(pressed > 0.5f)
        {
            OnCloseSaveMenuEvent?.Invoke();
        }
    }
    public void SwitchActionMap(string mapName)
    {
        menuInput.SwitchCurrentActionMap(mapName);
    }
}
