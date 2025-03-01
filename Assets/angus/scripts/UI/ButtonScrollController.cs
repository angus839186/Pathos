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

    public int selectedIndex = 0;

    public void SelectButton(int index)
    {
        if (index < 0 || index >= buttons.Count) return;

        selectedIndex = index;
        UpdateButtonPositions();
    }

    public void UpdateButtonPositions()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            RectTransform button = buttons[i];
            Vector2 targetPosition;

            if (i < selectedIndex) 
            {
                int offsetCount = selectedIndex - i;
                targetPosition = centerOffset + upOffset * offsetCount;
            }
            else if (i > selectedIndex) 
            {
                int offsetCount = i - selectedIndex;
                targetPosition = centerOffset + downOffset * offsetCount;
            }
            else 
            {
                targetPosition = centerOffset;
            }


            StartCoroutine(SmoothMove(button, targetPosition));
        }
    }

    private IEnumerator SmoothMove(RectTransform button, Vector2 targetPosition)
    {
        Vector2 startPosition = button.anchoredPosition;
        float elapsedTime = 0f;

        CanvasGroup cg = button.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = button.gameObject.AddComponent<CanvasGroup>();
        }

        float startAlpha = cg.alpha;
        float distance = Vector2.Distance(targetPosition, centerOffset);
        float targetAlpha = Mathf.Clamp01(1 - (distance / maxDistance));

        while (elapsedTime < animationSpeed)
        {
            float t = elapsedTime / animationSpeed;
            button.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        button.anchoredPosition = targetPosition;
        cg.alpha = targetAlpha;
    }
}
