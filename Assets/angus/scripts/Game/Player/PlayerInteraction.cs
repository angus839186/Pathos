using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor;
using System;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    // 用來顯示互動提示的 UI Text
    public GameObject interactHint;

    private IInteractable currentInteractable;

    public float interactInput;

    public Animator playerAnimator;

    public PlayerController player;

    public bool isInteracting;

    public Action<string> OnShowDescription;

    void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        PlayerInputManager.Instance.OnInteractEvent += Interact;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.OnInteractEvent -= Interact;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null && currentInteractable == interactable)
        {
            currentInteractable = null;
            interactHint.SetActive(false);
        }
    }

    public void Interact()
    {
        if (currentInteractable != null)
        {
            if (isInteracting)
                return;

            Item mainItem = Hotbar.Instance.mainItem;
            Item heldItem = (mainItem != null && mainItem != null) ? mainItem : null;

            string animTrigger = currentInteractable.GetAnimationTrigger(heldItem);
            if (!string.IsNullOrEmpty(animTrigger))
            {
                isInteracting = true;
                player.ToggleMove(false);
                playerAnimator.SetTrigger(animTrigger);
            }
            else
            {
                if (!string.IsNullOrEmpty(currentInteractable.GetDescription(heldItem)))
                {
                    OnShowDescription?.Invoke(currentInteractable.GetDescription(heldItem));
                }
                currentInteractable.Interact();
            }
        }
    }

    public void TriggerInteractEvent()
    {
        Item mainItem = Hotbar.Instance.mainItem;
        Item heldItem = (mainItem != null && mainItem != null) ? mainItem : null;
        if (currentInteractable != null)
        {
            StopInteracting();
            if (!string.IsNullOrEmpty(currentInteractable.GetDescription(heldItem)))
            {
                OnShowDescription?.Invoke(currentInteractable.GetDescription(heldItem));
            }
            currentInteractable.InteractEvent(heldItem);
        }
    }

    public void StopInteracting()
    {
        player.ToggleMove(true);
        isInteracting = false;
    }

    public void ToggleInteractingAnimation(bool toggle)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (toggle)
        {
            PlayerInputManager.Instance.SwitchActionMap("InteractingAnimation");
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
            PlayerInputManager.Instance.SwitchActionMap("Player");
        }
    }
}
