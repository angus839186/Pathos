using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public CanvasGroup currentCanva;

    public CanvasGroup mainMenuCanva;

    public CanvasGroup saveFileCanva;
    public float transitionDuration = 1f;
    [Header("First Selected Button")]
    [SerializeField] private Button firstSelected;

    void Start()
    {
        currentCanva = mainMenuCanva;

        // SetFirstSelected(firstSelected);
    }

    // public void SetFirstSelected(Button firstSelectedButton) 
    // {
    //     firstSelectedButton.Select();
    //     glowController buttonGlowEffect = firstSelectedButton.GetComponent<glowController>();
    //     buttonGlowEffect.StopAllCoroutines();
    //     buttonGlowEffect.StartCoroutine(buttonGlowEffect.GlowTransition(buttonGlowEffect.targetGlow));
    // }
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