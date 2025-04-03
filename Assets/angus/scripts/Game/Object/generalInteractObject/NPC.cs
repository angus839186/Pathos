using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractableObject
{
    public override string GetDescription()
    {
        return base.GetDescription();
    }

    public override string GetAnimationTrigger(Item heldItem)
    {
        return base.GetAnimationTrigger(null);
    }

    public override void Interact()
    {
        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                DialogueUI.Instance.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        DialogueUI.Instance.ShowDialogue(dialogueObject);
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do nothing
    }
}
