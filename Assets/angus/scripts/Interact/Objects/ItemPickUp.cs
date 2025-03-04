using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable, IDataPersistence
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

    public void LoadData(GameData data)
    {
        if (data.inventoryItemNames != null && data.inventoryItemNames.Contains(item.itemName))
        {
            // 道具已被玩家拾取，移除場景中的 ItemPickup
            Destroy(gameObject);
        }
    }

    public void SaveData(GameData data)
    {
        //Do Nothing
    }
}
