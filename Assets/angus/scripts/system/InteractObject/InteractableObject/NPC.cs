using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }
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

    public void InteractEvent(Item heldItem)
    {
        //Do nothing
    }
}
