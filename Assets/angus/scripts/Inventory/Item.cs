using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/新道具")]
public class Item : ScriptableObject
{
    public string itemName;    // 道具名稱
    public Sprite icon;        // 道具圖示

    public string itemDescription;  // 道具敘述
}
