using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    public string DefaultDescription;

    public virtual string GetDescription(Item heldItem)
    {
        return DefaultDescription;
    }

    public virtual string GetAnimationTrigger(Item heldItem)
    {
        return "";
    }

    public abstract void Interact();

    public abstract void InteractEvent(Item heldItem);

    public virtual void GiveItem(Item item)
    {
        InventoryManager.Instance.AddItem(item);
    }
}