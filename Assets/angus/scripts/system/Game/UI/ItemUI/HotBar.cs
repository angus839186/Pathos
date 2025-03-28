using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public static Hotbar Instance { get; private set; }

    public Image mainHotbarIcon;   // 主要道具的圖示

    public InventoryItem _item;

    public int currentMainItemIndex = 0;

    public Sprite DefaultSprite;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMainItem(InventoryItem item, int index)
    {
        if(item == _item || item == null)
        {
            _item = null;
            currentMainItemIndex = 0;
            mainHotbarIcon.sprite = DefaultSprite;
            return;
        }
        if (item != null)
        {
            _item = item;
            mainHotbarIcon.sprite = item.item.icon;
            currentMainItemIndex = index;
        }
    }
}
