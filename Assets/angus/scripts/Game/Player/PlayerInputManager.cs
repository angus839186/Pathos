using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    public PlayerInput playerInput;

    public event Action<Vector2> OnMoveEvent;
    public event Action<float> OnJumpEvent;
    public event Action<bool> OnRunEvent;

    public event Action OnToggleBackpackEvent;
    public event Action<int> OnSelectItemEvent;

    public event Action OnConfirmMainItemEvent;

    public event Action OnInteractEvent;

    public event Action OnCloseSaveMenuEvent;

    public event Action<int> OnSelectSaveFileEvent;

    public event Action OnNextTutorialEvent;

    public event Action<bool> OnToggleSettingMenuEvent;

    public event Action<bool> OnTogglePauseMenuEvent;

    public event Action OnContinueVideoEvent;

    public event Action<bool> OnClosePlayerTutorialEvent;

    public event Action OnNextDialogueEvent;

    public event Action BackToMenuEvent;

    public event Action<bool> OnPassVideoEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        OnMoveEvent?.Invoke(move);
    }

    void OnJump(InputValue value)
    {
        float jump = value.Get<float>();
        OnJumpEvent?.Invoke(jump);
    }

    void OnRun(InputValue value)
    {
        bool run = value.isPressed;
        OnRunEvent?.Invoke(run);
    }

    void OnOpenBackPack(InputValue value)
    {
        float pressed = value.Get<float>();
        if (value.Get<float>() > 0.5f)
        {
            OnToggleBackpackEvent?.Invoke();
        }
    }

    void OnCloseBackPack(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnToggleBackpackEvent?.Invoke();
        }
    }

    void OnInteract(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnInteractEvent?.Invoke();
        }
    }

    void OnUpSelectBackPackSlot(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnSelectItemEvent?.Invoke(-1);
        }
    }

    void OnDownSelectBackPackSlot(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnSelectItemEvent?.Invoke(1);
        }
    }

    void OnConfirmMainItem(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnConfirmMainItemEvent?.Invoke();
        }
    }
    void OnCloseSaveMenu(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnCloseSaveMenuEvent?.Invoke();
        }
    }

    void OnUpSelectSaveFileIndex(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnSelectSaveFileEvent?.Invoke(-1);
        }
    }

    void OnDownSelectSaveFileIndex(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnSelectSaveFileEvent?.Invoke(1);
        }
    }
    void OnNextTutorial(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnNextTutorialEvent?.Invoke();
        }
    }
    void OnCloseSettingMenu(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnToggleSettingMenuEvent?.Invoke(false);
        }
    }

    void OnOpenPauseMenu(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnTogglePauseMenuEvent?.Invoke(true);
        }
    }

    void OnClosePauseMenu(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnTogglePauseMenuEvent?.Invoke(false);
        }
    }

    void OnContinueVideo(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnContinueVideoEvent?.Invoke();
        }
    }

    void OnClosePlayerTutorial(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnClosePlayerTutorialEvent?.Invoke(false);
        }
    }

    void OnNextDialogue(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            OnNextDialogueEvent?.Invoke();
        }
    }

    void OnWaitForBackToMenu(InputValue value)
    {
        float pressed = value.Get<float>();
        if (pressed > 0.5f)
        {
            BackToMenuEvent?.Invoke();
        }
    }

    void OnPassVideo(InputValue value)
    {
        // value.Get<float>() 对于 Button 类型会是 1（按下时）或 0（放开时）
        bool pressed = value.Get<float>() > 0.5f;
        OnPassVideoEvent?.Invoke(pressed);
    }

    public void SwitchActionMap(string mapName)
    {
        playerInput.SwitchCurrentActionMap(mapName);
        Debug.Log("Switched to " + mapName);
    }
}
