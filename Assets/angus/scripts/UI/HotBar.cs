using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public static Hotbar Instance { get; private set; }

    public Image mainHotbarIcon;   // 主要道具的圖示

    public InventoryItem _item;

    public int currentMainItemIndex = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetMainItem(InventoryItem item, int index)
    {
        currentMainItemIndex = index;
        if (item != null && item.item != null)
        {
            _item = item;
            mainHotbarIcon.sprite = item.item.icon;
            // 設置圖示完全可見
            Color c = mainHotbarIcon.color;
            c.a = 1f;
            mainHotbarIcon.color = c;
        }
        else
        {
            // 當沒有道具時，將 _item 設為 null
            _item = null;
            mainHotbarIcon.sprite = null;
            // 設置圖示透明
            Color c = mainHotbarIcon.color;
            c.a = 0f;
            mainHotbarIcon.color = c;
        }
    }
}
