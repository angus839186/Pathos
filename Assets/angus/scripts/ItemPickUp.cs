using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item;       // 參照你定義的道具資料（ScriptableObject 或其他類別）
    public int quantity = 1;

    public string defaultDescription;

    public string GetAnimationTrigger(Item heldItem)
    {
        return "";
    }

    // 根據道具資料返回提示文字
    public string GetDescription()
    {
        return defaultDescription;
    }

    public void Interact(Item heldItem)
    {
        Inventory inventory = Inventory.Instance;
        if (inventory != null)
        {
            inventory.AddItem(item, quantity);
            // 撿取成功後移除場景中的物件
            Destroy(gameObject);
        }
    }
}
