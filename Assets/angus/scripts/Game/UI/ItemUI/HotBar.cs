using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public static Hotbar Instance { get; private set; }

    public Image mainHotbarIcon;   // 主要道具的圖示

    public Item mainItem;

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

    public void SetMainItem(Item item, int index)
    {
        if(item == mainItem || item == null)
        {
            mainItem = null;
            currentMainItemIndex = 0;
            mainHotbarIcon.sprite = DefaultSprite;
            return;
        }
        if (item != null)
        {
            mainItem = item;
            mainHotbarIcon.sprite = item.icon;
            currentMainItemIndex = index;
        }
    }
}
