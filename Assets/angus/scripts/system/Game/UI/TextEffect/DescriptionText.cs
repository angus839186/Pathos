using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionText : MonoBehaviour
{
    public Text descriptionText;

    private CanvasGroup canvas;
    PlayerInteraction playerInteract;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        playerInteract = FindObjectOfType<PlayerInteraction>();
        canvas.alpha = 0f;
        if(playerInteract != null)
        {
            playerInteract.OnShowDescription += showDescription;
        }
    }

    void OnDisable()
    {
        if(playerInteract != null)
        {
            playerInteract.OnShowDescription -= showDescription;
        }
    }

    public void showDescription(string description)
    {
        StartCoroutine(DisplayDescription(description));
    }
    public IEnumerator DisplayDescription(string description)
    {
        if(!string.IsNullOrEmpty(description))
        {
            canvas.alpha = 1f;
            descriptionText.text = description;
            yield return new WaitForSeconds(2.5f);
            canvas.alpha = 0f;
        }
    }
}
