using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public CanvasGroup mainMenuCanva;

    public CanvasGroup saveFileCanva;
    public CanvasGroup settingCanva;
    public float transitionDuration = 1f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        mainMenuCanva = MainMenu.Instance.GetComponent<CanvasGroup>();
        saveFileCanva = SaveFileMenu.Instance.GetComponent<CanvasGroup>();
        settingCanva = SettingMenu.Instance.GetComponent<CanvasGroup>();
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
            canvasGroup.alpha = Mathf.Lerp(1, 0, t);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canva.alpha = Mathf.Lerp(0, 1, t);
            canva.interactable = true;
            canva.blocksRaycasts = true;
            yield return null;
        }
        canvasGroup.alpha = 0;
        canva.alpha = 1;
    }
    public void ToggleCanvasGroup(bool toggle)
    {
        canvasGroup.alpha = toggle ? 1 : 0;
        canvasGroup.interactable = toggle;
        canvasGroup.blocksRaycasts = toggle;
    }
}