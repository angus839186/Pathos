using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScrollController : MonoBehaviour
{
    public List<RectTransform> buttons; 
    public float animationSpeed = 0.3f; 

    public RectTransform LoadDataCenter;
    public Vector2 centerOffset;
    public Vector2 upOffset;
    public Vector2 downOffset;

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
            Vector2 targetPosition;

            if (i < selectedIndex) // 上方按鈕
            {
                int offsetCount = selectedIndex - i;
                targetPosition = centerOffset + upOffset * offsetCount;
            }
            else if (i > selectedIndex) // 下方按鈕
            {
                int offsetCount = i - selectedIndex;
                targetPosition = centerOffset + downOffset * offsetCount;
            }
            else // 中間按鈕
            {
                targetPosition = centerOffset;
            }

            // 開始平滑移動
            StartCoroutine(SmoothMove(buttons[i], targetPosition));
        }
    }

    private IEnumerator SmoothMove(RectTransform button, Vector2 targetPosition)
    {
        Vector2 startPosition = button.anchoredPosition;
        float elapsedTime = 0;

        while (elapsedTime < animationSpeed)
        {
            button.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / animationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        button.anchoredPosition = targetPosition;
    }
}
