using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor;

public class PlayerInteraction : MonoBehaviour
{
    // 用來顯示互動提示的 UI Text
    public GameObject interactHint;

    public IInteractable currentInteractable;

    public float interactInput;

    public DescriptionText descriptionText;

    public Animator playerAnimator;

    public PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        // 訂閱 InputManager 的事件
        PlayerInputManager.Instance.OnInteractEvent += Interact;
    }

    void OnDisable()
    {
        // 訂閱要取消
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

    public void Interact(float value)
    {
        interactInput = value;
        if (currentInteractable != null && interactInput > 0.5f)
        {
            if (player.isInteracting)
                return;

            InventoryItem mainItem = Hotbar.Instance._item; // 此處 hotbar._item 可能為 null
            Item heldItem = (mainItem.item != null) ? mainItem.item : null;

            string animTrigger = currentInteractable.GetAnimationTrigger(heldItem);
            if (!string.IsNullOrEmpty(animTrigger))
            {
                playerAnimator.SetTrigger(animTrigger);
                player.isInteracting = true;
            }
            else
            {
                StartCoroutine(descriptionText.showDescription(currentInteractable.GetDescription()));
                currentInteractable.Interact();
            }
        }
    }

    public void TriggerInteractEvent()
    {
        InventoryItem mainItem = Hotbar.Instance._item; // 此處 hotbar._item 可能為 null
        Item heldItem = (mainItem != null) ? mainItem.item : null;
        if (currentInteractable != null)
        {
            currentInteractable.InteractEvent(heldItem);
        }
        if (player != null)
        {
            player.isInteracting = false;
        }
    }
}
