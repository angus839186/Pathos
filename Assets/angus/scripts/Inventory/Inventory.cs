using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory Instance { get; private set; }
    public List<InventoryItem> items = new List<InventoryItem>();

    // 定義事件，當 Inventory 改變時觸發
    public event Action OnInventoryChanged;

    private void Awake()
    {
        // 如果已存在其他實例，就銷毀新的物件
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // 若希望 Inventory 在場景切換時不被銷毀，可以使用 DontDestroyOnLoad
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddItem(Item newItem, int amount = 1)
    {
        InventoryItem invItem = items.Find(x => x.item == newItem);
        if (invItem != null)
        {
            invItem.quantity += amount;
        }
        else
        {
            InventoryItem newInvItem = new InventoryItem { item = newItem, quantity = amount };
            items.Add(newInvItem);
        }

        OnInventoryChanged?.Invoke();
        Debug.Log("得到新東西");
    }

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
            OnInventoryChanged?.Invoke();
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public Item item;      // 道具資料
    public int quantity;   // 數量
}
