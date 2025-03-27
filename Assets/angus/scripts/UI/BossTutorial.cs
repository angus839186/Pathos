using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTutorial : MonoBehaviour
{
    public int tutorialIndex = 0;
    public List<GameObject> tutorials;

    public Action<bool> StartBossFight;
    public bool playedTutorial;
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
        if (!playedTutorial)
        {
            PlayerInputManager.Instance.SwitchActionMap("BossTutorial");
            CanvasGroup canvas = GetComponent<CanvasGroup>();
            canvas.alpha = 1f;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
            tutorials[tutorialIndex].SetActive(true);
        }
        else
        {
            TutorialEnd();
        }
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
        PlayerInputManager.Instance.SwitchActionMap("Player");
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        StartBossFight.Invoke(false);
        ReversePlainBoss boss = FindObjectOfType<ReversePlainBoss>();
        boss.StartAttack();
    }
}
