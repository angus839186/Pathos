using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Interactions;

public class BackpackUIManager : MonoBehaviour
{
    public static BackpackUIManager Instance;
    [Header("UI 參考")]
    public CanvasGroup backpackPanel;
    public List<Image> bagSlots;

    public Image displayImage;
    public Text itemName;
    public Text itemDescription;

    [Header("輸入與資料")]
    public PlayerInputManager playerInput;
    public int currentIndex;
    private InventoryManager inventory;

    public bool isBackpackOpen = false;

    public Sprite previousSelectSprite;
    public Sprite currentSelectSprite;
    public Sprite nextSelectSprite;

    public Sprite noneSprite;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        playerInput = PlayerInputManager.Instance;
    }

    void OnEnable()
    {
        inventory = InventoryManager.Instance;
        playerInput = PlayerInputManager.Instance;


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
        backpackPanel.alpha = 1f;
        backpackPanel.interactable = true;
        backpackPanel.blocksRaycasts = true;
        playerInput.SwitchActionMap("Backpack");
        UpdateItemSlots();
        showItemDetail();
    }


    public void CloseBackpack()
    {
        backpackPanel.alpha = 0f;
        backpackPanel.interactable = false;
        backpackPanel.blocksRaycasts = false;
        playerInput.SwitchActionMap("Player");
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
            childImage.color = new Color(1, 1, 1, 1);
            if (i == currentIndex)
            {
                slotImage.sprite = currentSelectSprite;
                slotImage.color = new Color(1, 1, 1, 0);
                childImage.color = new Color(1, 1, 1, 0);
            }

            int previous = currentIndex - 1;
            if (i == previous && previous >= 0)
            {
                slotImage.sprite = previousSelectSprite;
            }

            int next = currentIndex + 1;
            if (i == next && next < inventory.items.Count)
            {
                slotImage.sprite = nextSelectSprite;
            }
        }
    }



    public void showItemDetail()
    {
        if (inventory.items.Count <= 0)
        {
            displayImage.sprite = noneSprite;
            itemName.text = "";
            itemDescription.text = "";
            return;
        }
        InventoryItem selectedItem = inventory.items[currentIndex];
        displayImage.sprite = selectedItem.item.icon;
        itemName.text = selectedItem.item.itemName;
        itemDescription.text = selectedItem.item.itemDescription;
    }

    public void OnConfirmItem()
    {
        int newIndex = currentIndex;
        InventoryItem selectedItem = inventory.items[currentIndex];
        if(selectedItem != null)
        {
            Hotbar.Instance.SetMainItem(selectedItem, currentIndex);
        }

        currentIndex = newIndex;
        CloseBackpack();
        isBackpackOpen = false;
    }
}
