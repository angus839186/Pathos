using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionText : MonoBehaviour
{
    public Text descriptionText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerInteraction playerInteract = FindObjectOfType<PlayerInteraction>();
        Debug.Log(playerInteract);
        playerInteract.OnShowDescription += showDescription;
    }

    void OnDisable()
    {
        PlayerInteraction playerInteract = FindObjectOfType<PlayerInteraction>();
        playerInteract.OnShowDescription -= showDescription;
    }

    public void showDescription(string description)
    {
        StartCoroutine(DisplayDescription(description));
    }
    public IEnumerator DisplayDescription(string description)
    {
        this.gameObject.SetActive(true);
        descriptionText.text = description;
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }
}
