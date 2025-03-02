using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IDataPersistence
{

    public static InventoryManager Instance { get; private set; }
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

    public void AddItem(Item newItem)
    {
        InventoryItem invItem = items.Find(x => x.item == newItem);
        if (invItem != null)
        {
            return;
        }
        else
        {
            InventoryItem newInvItem = new InventoryItem { item = newItem };
            items.Add(newInvItem);
        }

        OnInventoryChanged?.Invoke();
        Debug.Log("得到新東西");
    }

    public void RemoveItem(Item item)
    {
        InventoryItem invItem = items.Find(x => x.item == item);
        if (invItem != null)
        {
            items.Remove(invItem);
            OnInventoryChanged?.Invoke();
        }
    }

    public void SaveData(ref GameData data)
    {
        // 確保欄位存在
        if (data.inventoryItemNames == null)
        {
            data.inventoryItemNames = new List<string>();
        }
        else
        {
            data.inventoryItemNames.Clear();
        }

        foreach (InventoryItem invItem in items)
        {
            // 假設每個 Item 的 itemName 為唯一識別字串
            data.inventoryItemNames.Add(invItem.item.itemName);
        }
    }

    public void LoadData(GameData data)
    {
        if (data.inventoryItemNames != null)
        {
            items.Clear();
            foreach (string itemName in data.inventoryItemNames)
            {
                // 假設所有 Item 都放在 Resources/Items 資料夾中
                Item item = Resources.Load<Item>("Items/" + itemName);
                if (item != null)
                {
                    InventoryItem newInvItem = new InventoryItem { item = item };
                    items.Add(newInvItem);
                }
                else
                {
                    Debug.LogWarning("找不到 Item: " + itemName);
                }
            }
            OnInventoryChanged?.Invoke();
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public Item item;
}
