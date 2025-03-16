using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fence : MonoBehaviour, IInteractable
{
    public string DefaultDescription;
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
        throw new System.NotImplementedException();
    }
}
