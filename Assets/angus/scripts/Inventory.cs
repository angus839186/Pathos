using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    // 加入道具（如果背包中已存在同種道具，則累加數量）
    public void AddItem(Item newItem, int amount = 1)
    {
        InventoryItem invItem = items.Find(x => x.item == newItem);
        if (invItem != null)
        {
            invItem.quantity += amount;
        }
        else
        {
            InventoryItem newInvItem = new InventoryItem();
            newInvItem.item = newItem;
            newInvItem.quantity = amount;
            items.Add(newInvItem);
        }
        // 可在此處呼叫更新 UI 的方法
    }

    // 移除道具（若數量歸零則從列表中刪除）
    public void RemoveItem(Item item, int amount = 1)
    {
        InventoryItem invItem = items.Find(x => x.item == item);
        if (invItem != null)
        {
            invItem.quantity -= amount;
            if (invItem.quantity <= 0)
            {
                items.Remove(invItem);
            }
        }
        // 更新 UI
    }
}
[System.Serializable]
public class InventoryItem
{
    public Item item;      // 道具資料
    public int quantity;   // 數量
}
