using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundButton : MonoBehaviour, IPointerEnterHandler
{
    public Sprite targetBackground;  // 指定按鈕對應的新背景

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 呼叫背景管理器改變背景圖片
        BackgroundManager.Instance.ChangeBackground(targetBackground);
    }
}