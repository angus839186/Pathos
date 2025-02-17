using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganDoor : MonoBehaviour, IInteractable
{
    public string DefaultDescription;

    public Sprite closedSprite;

    public AudioClip sound;
    public string GetAnimationTrigger(Item heldItem)
    {
        return "";
    }

    public string GetDescription()
    {
        return DefaultDescription;
    }

    public void Interact()
    {
        Debug.Log(DefaultDescription);
    }

    public void InteractEvent(Item heldItem)
    {
        return;
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
