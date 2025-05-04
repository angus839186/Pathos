using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableNPC : MonoBehaviour, IInteractable
{
    public DialogueObject dialogueObject;

    public AudioClip InteractSound;
    private ResponseEventManager responseEventManager;
    private ResponseHandler responseHandler;

    private DescriptionText descriptionText;

    void Awake()
    {
        responseEventManager = FindObjectOfType<ResponseEventManager>();
        responseHandler = FindObjectOfType<ResponseHandler>();
        descriptionText = FindObjectOfType<DescriptionText>();
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
        if (InteractSound != null)
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

    public void ShowMissionHint(string msg)
    {
        descriptionText.showDescription(msg);
    }

    public virtual void ToggleCrutch(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.拐杖, toggle);
            string msg = toggle
            ? "開啟話題『拐杖』"
            : "關閉話題『拐杖』。";  
            ShowMissionHint(msg);
        }
    }

    public virtual void ToggleMusicScore(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.樂譜, toggle);
            string msg = toggle
            ? "開啟話題『樂譜』"
            : "關閉話題『樂譜』。";  
            ShowMissionHint(msg);
        }
    }

    public virtual void ToggleMusicianShow(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.樂手表演, toggle);
            string msg = toggle
            ? "開啟話題『樂手表演』"
            : "關閉話題『樂手表演』。";  
            ShowMissionHint(msg);

        }
    }

    public virtual void ToggleMusicFlower(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.音樂花, toggle);
            string msg = toggle
            ? "開啟話題『音樂花』"
            : "關閉話題『音樂花』。";  
            ShowMissionHint(msg);
        }
    }

    public virtual void ToggleMissingVillager(bool toggle)
    {
        if (responseEventManager != null)
        {
            responseEventManager.SetResponseTypeState(ResponseEventType.失蹤村民, toggle);
            string msg = toggle
            ? "開啟話題『失蹤村民』"
            : "關閉話題『失蹤村民』。";
            ShowMissionHint(msg);
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
