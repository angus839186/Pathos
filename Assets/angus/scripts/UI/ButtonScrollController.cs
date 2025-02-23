using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScrollController : MonoBehaviour
{
    public List<RectTransform> buttons; 
    public float animationSpeed;

    public RectTransform LoadDataCenter;
    public Vector2 centerOffset;
    public Vector2 upOffset;
    public Vector2 downOffset;

    public float maxDistance = 200f;

    private int selectedIndex = 0; 

    void Start()
    {
        centerOffset = new Vector2(LoadDataCenter.anchoredPosition.x, LoadDataCenter.anchoredPosition.y);
        UpdateButtonPositions();
    }

    public void SelectButton(int index)
    {
        if (index < 0 || index >= buttons.Count) return;

        selectedIndex = index;
        UpdateButtonPositions();
    }

    private void UpdateButtonPositions()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            RectTransform button = buttons[i];
            Vector2 targetPosition;

            if (i < selectedIndex) // 上方的按鈕
            {
                int offsetCount = selectedIndex - i;
                targetPosition = centerOffset + upOffset * offsetCount;
            }
            else if (i > selectedIndex) // 下方的按鈕
            {
                int offsetCount = i - selectedIndex;
                targetPosition = centerOffset + downOffset * offsetCount;
            }
            else // 中間按鈕（選取的按鈕）
            {
                targetPosition = centerOffset;
            }

            // 同時插值位置與透明度
            StartCoroutine(SmoothMove(button, targetPosition));
        }
    }

    private IEnumerator SmoothMove(RectTransform button, Vector2 targetPosition)
    {
        Vector2 startPosition = button.anchoredPosition;
        float elapsedTime = 0f;
        // 確保有 CanvasGroup 用來控制透明度
        CanvasGroup cg = button.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = button.gameObject.AddComponent<CanvasGroup>();
        }
        // 取得當前透明度
        float startAlpha = cg.alpha;
        // 根據目標位置與中心點距離計算目標透明度
        float distance = Vector2.Distance(targetPosition, centerOffset);
        float targetAlpha = Mathf.Clamp01(1 - (distance / maxDistance));

        while (elapsedTime < animationSpeed)
        {
            float t = elapsedTime / animationSpeed;
            // 插值位置
            button.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            // 插值透明度
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        button.anchoredPosition = targetPosition;
        cg.alpha = targetAlpha;
    }
}
