using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    // 假設這裡 hotbarItems 用來顯示在 UI 上，這裡可以直接參考 Inventory 的 items
    public List<InventoryItem> hotbarItems = new List<InventoryItem>();

    public Inventory inventory; // 在 Inspector 中拖入 Inventory 物件
    public List<Image> hotbarSlots;

    public int selectedIndex = -1;

    void Start()
    {
        // 訂閱 Inventory 的變動事件
        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateHotbarItems;
            UpdateHotbarItems(); // 初始更新
        }
    }

    void UpdateHotbarItems()
    {
        hotbarItems = new List<InventoryItem>(inventory.items);

        // 然後更新 UI，例如更新各個 slot 的圖示與數量
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            if (i < hotbarItems.Count)
            {
                hotbarSlots[i].sprite = hotbarItems[i].item.icon;
                // 可再更新數量的文字等
            }
            else
            {
                hotbarSlots[i].sprite = null;
            }
        }
    }

    void Update()
    {
        // 處理數字鍵選取邏輯
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (selectedIndex == i)
                    selectedIndex = -1;
                else
                    selectedIndex = i;

                UpdateHotbarUI();
            }
        }
    }

    void UpdateHotbarUI()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            hotbarSlots[i].color = (i == selectedIndex) ? Color.yellow : Color.white;
        }
    }

    // 用來獲取目前選取的道具
    public InventoryItem GetCurrentSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < hotbarItems.Count)
            return hotbarItems[selectedIndex];
        return null;
    }
}
