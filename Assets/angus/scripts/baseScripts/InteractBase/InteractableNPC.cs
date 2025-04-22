using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableNPC : MonoBehaviour, IInteractable
{
    public DialogueObject dialogueObject;

    public AudioClip InteractSound;
    private ResponseEventManager responseEventManager;
    private ResponseHandler responseHandler;

    void Awake()
    {
        responseEventManager = FindObjectOfType<ResponseEventManager>();
        responseHandler = FindObjectOfType<ResponseHandler>();
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
        responseHandler.Npc = this;
        UpdateDialogueObject(GetDialogue());
        if(InteractSound != null)
        {
            AudioManager.instance.PlaySound(InteractSound);
        }
        GetNewResponseEvent(GetDialogue());

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
            Debug.Log("拐杖任務已更新。");
        }
    }

    public virtual void ToggleMusicScore(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.樂譜, toggle);
            Debug.Log($"樂譜任務已更新,{toggle}");
        }
    }

    public virtual void ToggleMusicianShow(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.樂手表演, toggle);
            Debug.Log("樂手表演任務已更新。");
        }
    }

    public virtual void ToggleMusicFlower(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.音樂花, toggle);
            Debug.Log("音樂花任務已更新。");
        }
    }

    public virtual void ToggleMissingVillager(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.失蹤村民, toggle);
            Debug.Log("失蹤村民任務已更新。");
        }
    }


    public virtual DialogueObject GetDialogue()
    {
        return dialogueObject;
    }

    public void GetNewResponseEvent(DialogueObject dialogueObject)
    {
        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                DialogueUI.Instance.AddResponseEvents(responseEvents.Events);
                break;
            }
        }
    }
}
