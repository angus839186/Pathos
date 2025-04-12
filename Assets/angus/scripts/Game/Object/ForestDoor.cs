using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestDoor : InteractableObject
{
    public Animator anime;

    public AudioClip sound;

    public bool ForestDoorOpen;

    void Start()
    {
        anime = GetComponent<Animator>();
    }
    public override string GetAnimationTrigger(Item heldItem)
    {
        return base.GetAnimationTrigger(null);
    }

    public override string GetDescription(Item heldItem)
    {
        return base.GetDescription(null);
    }

    public override void Interact()
    {
        Debug.Log("Go To Forest");
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do Nothing
    }
    public void Open()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        anime.SetTrigger("OpenForestDoor");
        collider.enabled = true;
        ForestDoorOpen = true;
        AudioManager.instance.PlaySound(sound);
        
    }
}
