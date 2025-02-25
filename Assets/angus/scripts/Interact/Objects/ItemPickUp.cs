using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item;

    public string defaultDescription;

    public string GetAnimationTrigger(Item heldItem)
    {
        return "";
    }

    public string GetDescription()
    {
        return defaultDescription;
    }

    public void Interact()
    {
        InventoryManager inventory = InventoryManager.Instance;
        if (inventory != null)
        {
            inventory.AddItem(item);

            Destroy(gameObject);
        }
    }

    public void InteractEvent(Item heldItem)
    {
        throw new System.NotImplementedException();
    }
}
