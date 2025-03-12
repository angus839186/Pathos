using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int tutorialIndex = 0;
    public List<GameObject> tutorials;
    void OnEnable()
    {
        PlayerInputManager.Instance.OnNextTutorialEvent += NextTutorial;
    }
    void OnDisable()
    {
        PlayerInputManager.Instance.OnNextTutorialEvent -= NextTutorial;
    }

    public void InitializeTutorial()
    {
        PlayerInputManager.Instance.SwitchActionMap("Tutorial");
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 1f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        tutorials[tutorialIndex].SetActive(true);
    }

    public void NextTutorial()
    {
        tutorials[tutorialIndex].SetActive(false);
        tutorialIndex++;
        if (tutorialIndex >= tutorials.Count)
        {
            TutorialEnd();
            return;
        }
        tutorials[tutorialIndex].SetActive(true);
    }
    public void TutorialEnd()
    {
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        PlayerInputManager.Instance.SwitchActionMap("Player");
        ReversePlainBoss boss = FindObjectOfType<ReversePlainBoss>();
        boss.StartAttack();
    }
}
