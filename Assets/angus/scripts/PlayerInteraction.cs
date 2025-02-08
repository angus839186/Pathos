using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // 連結 Hotbar（用以取得玩家目前選取的道具）
    public Hotbar hotbar;
    // 用來顯示互動提示的 UI Text
    public GameObject interactHint;

    public IInteractable currentInteractable;

    public float interactInput;

    public DescriptionText descriptionText;

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

    public void OnInteract(InputValue inputValue)
    {
        interactInput = inputValue.Get<float>();
        if (currentInteractable != null && interactInput > 0.5f)
        {

            InventoryItem selectedItem = hotbar.GetCurrentSelectedItem();
            Item heldItem = (selectedItem != null) ? selectedItem.item : null;

            descriptionText.showDescription(currentInteractable.GetDescription());

            currentInteractable.Interact(heldItem);
        }
    }
}
