using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundButton : MonoBehaviour, IPointerEnterHandler
{
    public Sprite targetBackground;  // 指定按鈕對應的新背景

    public void OnPointerEnter(PointerEventData eventData)
    {
        BackgroundManager.Instance.ChangeBackground(targetBackground);
    }
}