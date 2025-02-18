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

    public event Action<float> OnConfirmMainItemEvent;

    public event Action<float> OnInteractEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 如果還沒指定，試著從本物件取得
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        OnMoveEvent?.Invoke(move);
    }

    public void OnJump(InputValue value)
    {
        float jump = value.Get<float>();
        OnJumpEvent?.Invoke(jump);
    }

    public void OnRun(InputValue value)
    {
        bool run = value.isPressed;
        OnRunEvent?.Invoke(run);
    }

    public void OnOpenBackPack(InputValue value)
    {
        float pressed = value.Get<float>();
        if (value.Get<float>() > 0.5f)
        {
            OnToggleBackpackEvent?.Invoke();
        }
    }

    public void OnCloseBackPack(InputValue value)
    {
        float pressed = value.Get<float>();
        if(value.Get<float>() > 0.5f)
        {
            OnToggleBackpackEvent?.Invoke();
        }
    }

    public void OnInteract(InputValue value)
    {
        float pressed = value.Get<float>();
        if (value.Get<float>() > 0.5f)
        {
            OnInteractEvent?.Invoke(pressed);
        }
    }

    public void OnUpSelect(InputValue value)
    {
        float pressed = value.Get<float>();
        if (value.Get<float>() > 0.5f)
        {
            OnSelectItemEvent?.Invoke(-1);
        }
    }

    public void OnDownSelect(InputValue value)
    {
        float pressed = value.Get<float>();
        if (value.Get<float>() > 0.5f)
        {
            OnSelectItemEvent?.Invoke(1);
        }
    }

    public void OnConfirmMainItem(InputValue value)
    {
        if (value.Get<float>() > 0.5f)
        {
            OnConfirmMainItemEvent?.Invoke(value.Get<float>());
        }
    }

    public void SwitchActionMap(string mapName)
    {
        playerInput.SwitchCurrentActionMap(mapName);
    }
}
