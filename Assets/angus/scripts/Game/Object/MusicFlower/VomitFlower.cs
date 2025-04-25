using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VomitFlower : InteractableObject
{
    [SerializeField] Animator anime;
    [SerializeField] private string musicPassword;

    public string answer;

    private bool passed;

    public AudioClip flowerBruhSound;

    public string VomitHint;

    [SerializeField] GameObject Vomit;
     private int currentIndex;
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    public void CheckMusicOrder(int number)
    {
        if (passed) return;
        if (number.ToString()[0] == musicPassword[currentIndex])
        {
            currentIndex++;
            answer += number;
        }
        else
        {
            currentIndex = 0;
            answer = "";
        }
        if (currentIndex >= musicPassword.Length)
        {
            anime.SetTrigger("Vomit");
            DescriptionText description = FindObjectOfType<DescriptionText>();
            description.showDescription(VomitHint);
            AudioManager.instance.PlaySound(flowerBruhSound);
            passed = true;
        }
    }
    public void ShowVomit()
    {
        Vomit.SetActive(true);
    }

    public override void Interact()
    {
        //Do nothing
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do nothing
    }
}
