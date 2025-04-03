using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganDoor : InteractableObject
{
    public Sprite closedSprite;

    public AudioClip sound;
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
        //Do Nothing
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do Nothing
    }
    public void Open()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        collider.enabled = false;
        sprite.sprite = closedSprite;
        AudioManager.instance.PlaySound(sound);
        
    }
}
