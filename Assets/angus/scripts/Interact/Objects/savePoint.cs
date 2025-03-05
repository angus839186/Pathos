using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class savePoint : MonoBehaviour, IInteractable
{
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
        SaveFileMenu.Instance.ActivateSaveFileCanvas();
    }

    public void InteractEvent(Item heldItem)
    {
        //Do nothing
    }
}
