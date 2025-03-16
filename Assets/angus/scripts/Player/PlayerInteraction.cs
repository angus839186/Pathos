using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor;
using System;

public class PlayerInteraction : MonoBehaviour
{
    // 用來顯示互動提示的 UI Text
    public GameObject interactHint;

    public IInteractable currentInteractable;

    public float interactInput;

    public DescriptionText descriptionText;

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

            InventoryItem mainItem = Hotbar.Instance._item;
            Item heldItem = (mainItem != null && mainItem.item != null) ? mainItem.item : null;

            string animTrigger = currentInteractable.GetAnimationTrigger(heldItem);
            if (!string.IsNullOrEmpty(animTrigger))
            {
                playerAnimator.SetTrigger(animTrigger);
                isInteracting = true;
                player.canMove = false;
            }
            else
            {
                OnShowDescription?.Invoke(currentInteractable.GetDescription());
                currentInteractable.Interact();
            }
        }
    }

    public void TriggerInteractEvent()
    {
        InventoryItem mainItem = Hotbar.Instance._item;
        Item heldItem = (mainItem != null && mainItem.item != null) ? mainItem.item : null;
        if (currentInteractable != null)
        {
            currentInteractable.InteractEvent(heldItem);
            OnShowDescription?.Invoke(currentInteractable.GetDescription());
        }
        if (player != null)
        {
            player.canMove = true;
            isInteracting = false;
        }
    }
}
