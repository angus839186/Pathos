using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableNPC : MonoBehaviour, IInteractable
{
    public DialogueObject dialogueObject;

    public virtual string GetDescription(Item heldItem)
    {
        return "";
    }

    public virtual string GetAnimationTrigger(Item heldItem)
    {
        return "";
    }
    public virtual void Interact()
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

    public virtual void InteractEvent(Item heldItem)
    {
        //Do nothing
    }

    public virtual void GiveItem(Item item)
    {
        InventoryManager.Instance.AddItem(item);
    }

    public virtual void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }
}
