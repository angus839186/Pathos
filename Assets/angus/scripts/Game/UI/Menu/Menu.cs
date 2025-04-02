using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static CanvasGroup currentCanva;

    public CanvasGroup mainMenuCanva;

    public CanvasGroup saveFileCanva;

    public CanvasGroup settingCanva;
    public float transitionDuration = 1f;

    void Start()
    {
        currentCanva = mainMenuCanva;
    }
    public void Transition(CanvasGroup newCanvas)
    {
        StartCoroutine(TransitionCoroutine(newCanvas));
    }

    IEnumerator TransitionCoroutine(CanvasGroup canva)
    {
        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            currentCanva.alpha = Mathf.Lerp(1, 0, t);
            currentCanva.interactable = false;
            currentCanva.blocksRaycasts = false;
            canva.alpha = Mathf.Lerp(0, 1, t);
            canva.interactable = true;
            canva.blocksRaycasts = true;
            yield return null;
        }
        currentCanva.alpha = 0;
        canva.alpha = 1;
        currentCanva = canva;
    }
}