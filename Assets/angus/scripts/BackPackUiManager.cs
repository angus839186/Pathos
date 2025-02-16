using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class BackpackUIManager : MonoBehaviour
{
    [Header("UI 參考")]
    public GameObject backpackPanel;
    public List<Image> bagSlots;

    public Image displayImage;
    public Text itemName;
    public Text itemDescription;

    [Header("輸入與資料")]
    public PlayerInputManager playerInput;
    public int currentIndex;
    private Inventory inventory;

    public bool isBackpackOpen = false;

    public Sprite previousSelectSprite;
    public Sprite currentSelectSprite;
    public Sprite nextSelectSprite;

    void Awake()
    {
        inventory = Inventory.Instance;
        playerInput = PlayerInputManager.Instance;
    }

    void Start()
    {
        backpackPanel.SetActive(false);
        for (int i = 0; i < bagSlots.Count; i++)
        {
            Image[] images = bagSlots[i].GetComponentsInChildren<Image>();
            Image childImage = images[1];
            bagSlots[i].sprite = currentSelectSprite;
            childImage.color = new Color(1, 1, 1, 0);
        }
    }

    void OnEnable()
    {
        playerInput.OnToggleBackpackEvent += OnToggleBackpack;
        playerInput.OnSelectItemEvent += SelectItem;
        playerInput.OnConfirmMainItemEvent += OnConfirmItem;
        inventory.OnInventoryChanged += UpdateItemSlotsSprite;
    }

    void OnDisable()
    {
        playerInput.OnToggleBackpackEvent -= OnToggleBackpack;
        playerInput.OnSelectItemEvent -= SelectItem;
        playerInput.OnConfirmMainItemEvent -= OnConfirmItem;
        inventory.OnInventoryChanged -= UpdateItemSlotsSprite;
    }


    public void OpenBackpack()
    {
        backpackPanel.SetActive(true);
        playerInput.SwitchActionMap("Backpack");
        SetCurrentItem();
    }


    public void CloseBackpack()
    {
        backpackPanel.SetActive(false);
        playerInput.SwitchActionMap("DefaultPlayer");
    }


    public void OnToggleBackpack()
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

    public void SetCurrentItem()
    {
        UpdateItemSlots();
    }


    public void SelectItem(int value)
    {
        if (inventory.items.Count == 0) return;


        currentIndex += value;


        if (currentIndex < 0)
        {
            currentIndex = inventory.items.Count - 1;
        }
        else if (currentIndex >= inventory.items.Count)
        {
            currentIndex = 0;
        }

        UpdateItemSlots();

        showItemDetail();
    }


//更新物品欄物品圖案
    void UpdateItemSlotsSprite()
    {
        if (inventory.items.Count <= 0)
            return;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            Image[] images = bagSlots[i].GetComponentsInChildren<Image>();
            Image childImage = images[1];
            childImage.sprite = inventory.items[i].item.icon;
            if (childImage.sprite != null)
            {
                childImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                childImage.color = new Color(1, 1, 1, 0);
            }
        }
    }

    //更新物品欄格子
    public void UpdateItemSlots()
    {
        for (int i = 0; i < bagSlots.Count; i++)
        {
            Image slotImage = bagSlots[i];
            Image[] images = bagSlots[i].GetComponentsInChildren<Image>();
            Image childImage = images[1];

            slotImage.sprite = currentSelectSprite;
            slotImage.color = new Color(1, 1, 1, 1);

            if(childImage.sprite == null)
            {
                childImage.color = new Color(1, 1, 1, 0);
            }
            else
            {
                childImage.color = new Color(1, 1, 1, 1);
            }
            if (i == currentIndex)
            {
                slotImage.sprite = currentSelectSprite;
                slotImage.color = new Color(1, 1, 1, 0);
                childImage.color = new Color(1, 1, 1, 0);
            }

            int previous = currentIndex -1;
            if(i == previous && previous >= 0)
            {
                slotImage.sprite = previousSelectSprite;
            }

            int next = currentIndex +1;
            if(i == next && next < inventory.items.Count)
            {
                slotImage.sprite = nextSelectSprite;
            }
        }
    }



    public void showItemDetail()
    {
        if (inventory.items.Count <= 0)
        {
            displayImage.sprite = null;
            itemName.text = "";
            itemDescription.text = "";
            return;
        }
        InventoryItem selectedItem = inventory.items[currentIndex];
        displayImage.sprite = selectedItem.item.icon;
        itemName.text = selectedItem.item.itemName;
        itemDescription.text = selectedItem.item.itemDescription;
    }

    public void OnConfirmItem(float value)
    {
        int newIndex = currentIndex;
        InventoryItem selectedItem = inventory.items[currentIndex];
        if (Hotbar.Instance != null)
        {
            Hotbar.Instance.SetMainItem(selectedItem, currentIndex);
        }

        currentIndex = newIndex;
        CloseBackpack();
        isBackpackOpen = false;
    }
}
