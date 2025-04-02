using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicFlower : InteractableObject
{

    [SerializeField] private Animator anime;
    [SerializeField] int musicNumber;

    private VomitFlower vomitFlower;

    [SerializeField] AudioClip sound;

    void Awake()
    {
        anime = GetComponent<Animator>();
        vomitFlower = FindObjectOfType<VomitFlower>();
    }
    public override string GetAnimationTrigger(Item heldItem)
    {
        return "hitFlower";
    }
    public override void Interact()
    {

    }

    public override void InteractEvent(Item heldItem)
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        anime.SetTrigger("Shake");
        AudioManager.instance.PlaySound(sound);
        vomitFlower.CheckMusicOrder(musicNumber);
    }
}
