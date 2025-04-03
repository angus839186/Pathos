using UnityEngine;

public class ItemPickup : InteractableObject, IDataPersistence
{
    public Item itemToTake;
    public override string GetAnimationTrigger(Item heldItem)
    {
        return base.GetAnimationTrigger(null);
    }

    public override string GetDescription(Item heldItem)
    {
        return base.GetDescription(null);
    }

    public override void Interact()
    {
        InventoryManager inventory = InventoryManager.Instance;
        if (inventory != null)
        {
            inventory.AddItem(itemToTake);

            Destroy(gameObject);
        }
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do Nothing
    }

    public void LoadData(GameData data)
    {
        if (data.inventoryItemNames != null && data.inventoryItemNames.Contains(itemToTake.itemName))
        {
            // 道具已被玩家拾取，移除場景中的 ItemPickup
            Destroy(gameObject);
        }
    }

    public void SaveData(ref GameData data)
    {
        //Do Nothing
    }
}
