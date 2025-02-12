using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganDoor : MonoBehaviour, IInteractable
{
    public string DefaultDescription;

    public Sprite closedSprite;
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
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        if(collider != null)
        {
            collider.enabled = false;
        }
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        if(sprite != null)
        {
            sprite.sprite = closedSprite;
        }


    }
}
