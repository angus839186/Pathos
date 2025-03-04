using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class savePoint : MonoBehaviour, IInteractable
{
    public CanvasGroup SaveFileCanvas;
    public string GetAnimationTrigger(Item heldItem)
    {
        return "";
    }

    public string GetDescription()
    {
        return "";
    }

    public void Interact()
    {
        
    }

    public void InteractEvent(Item heldItem)
    {
        //Do nothing
    }
}
