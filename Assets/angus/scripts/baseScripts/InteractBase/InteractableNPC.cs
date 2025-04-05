using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableNPC : MonoBehaviour, IInteractable
{
    public DialogueObject dialogueObject;
    private ResponseEventManager responseEventManager;

    void Awake()
    {
        responseEventManager = FindObjectOfType<ResponseEventManager>();
    }

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

    public Item CheckItemOnPlayer(Item item)
    {
        return Hotbar.Instance.mainItem == item ? Hotbar.Instance.mainItem : null;
    }

    public virtual void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    public virtual void ToggleCrutch(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.拐杖, toggle);
            Debug.Log("拐杖狀態已更新。");
        }
    }

    public virtual void ToggleMusicScore(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.樂譜, toggle);
            Debug.Log($"樂譜狀態已更新,{toggle}");
        }
    }

    public virtual void ToggleMusicianShow(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.樂手表演, toggle);
            Debug.Log("樂譜狀態已更新。");
        }
    }


    public virtual DialogueObject GetDialogue()
    {
        return dialogueObject;
    }
}
