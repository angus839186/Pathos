using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class BackpackUIManager : MonoBehaviour
{
    [Header("UI 參考")]
    public GameObject backpackPanel;   // 背包 UI 面板
    public Image itemIcon;             // 顯示物品圖示的 UI 元件

    public Text itemName;              // 顯示物品名稱的 UI 元件
    public Text itemDescription;       // 顯示物品敘述的 UI 元件

    [Header("輸入與資料")]
    public PlayerInput playerInput;    // 參考全域的 PlayerInput
    private int currentIndex = 0;      // 當前選取的物品索引
    private Inventory inventory;       // 假設 Inventory 為 Singleton

    public bool isBackpackOpen = false;

    void Awake()
    {
        inventory = Inventory.Instance; // 從 Inventory 中取得資料
    }

    void Start()
    {
        backpackPanel.SetActive(false);
    }

    void OnEnable()
    {
        PlayerInputManager.Instance.OnToggleBackpackEvent += OnToggleBackpack;
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnToggleBackpackEvent -= OnToggleBackpack;
    }

    // 開啟背包時切換至 Backpack Action Map
    public void OpenBackpack()
    {
        backpackPanel.SetActive(true);
        playerInput.SwitchCurrentActionMap("Backpack");
        UpdateUI();
    }

    // 關閉背包時切換回 DefaultPlayer Action Map
    public void CloseBackpack()
    {
        backpackPanel.SetActive(false);
        playerInput.SwitchCurrentActionMap("DefaultPlayer");
    }

    // 使用 OnToggleBackpack 來切換背包狀態
    public void OnToggleBackpack(float value)
    {
        // 這邊只在按下事件時切換狀態
        if (value > 0.5f)
        {
            isBackpackOpen = !isBackpackOpen;
            if (isBackpackOpen)
            {
                OpenBackpack();
            }
            else
            {
                CloseBackpack();
            }
        }
    }

    // 更新 UI，顯示目前選取物品的圖示與敘述
    void UpdateUI()
{
    // 檢查 Inventory 與 items 是否存在且有物品
    if (inventory != null && inventory.items != null && inventory.items.Count > 0)
    {
        // 確保 currentIndex 在合法範圍內
        currentIndex = Mathf.Clamp(currentIndex, 0, inventory.items.Count - 1);
        InventoryItem item = inventory.items[currentIndex];
        if (item != null && item.item != null)
        {
            itemIcon.sprite = item.item.icon;
            itemName.text = item.item.itemName;
            // 若有敘述則顯示敘述，否則顯示物品名稱
            itemDescription.text = string.IsNullOrEmpty(item.item.itemDescription) ? 
            item.item.itemName : item.item.itemDescription;
        }
        else
        {
            itemIcon.sprite = null;
            itemName.text = "";
            itemDescription.text = "";
        }
    }
    else
    {
        // 當沒有物品時，重置索引並清空 UI
        currentIndex = 0;
        itemIcon.sprite = null;
        itemName.text = "";
        itemDescription.text = "";
    }
}
}
