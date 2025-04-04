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
        UpdateDialogueObject(GetDialogue());
        Debug.Log(GetDialogue().name);
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

    public virtual void RemoveItem(Item item)
    {
        InventoryManager.Instance.RemoveItem(item);
    }

    public virtual void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }
    public virtual void OpenResponseType(ResponseEventType responseType)
    {
        ResponseEventManager responseEventManager = FindObjectOfType<ResponseEventManager>();
        if(responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(responseType, true);
            Debug.Log("Crutch option has been enabled.");
        }
    }

    public virtual void CloseResponseType(ResponseEventType responseType)
    {
        ResponseEventManager responseEventManager = FindObjectOfType<ResponseEventManager>();
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(responseType, false);
            Debug.Log("Crutch option has been disabled.");
        }
    }

    public virtual DialogueObject GetDialogue()
    {
        return dialogueObject;
    }
}
