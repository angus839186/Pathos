using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageHeader : InteractableNPC
{
    public Item crutchItem;

    public bool getCrutch;

    public DialogueObject waitForCrutchDialogue;

    public DialogueObject getCrutchDialogue;

    public DialogueObject endingDialogue;
    public void OpenCrutchResponseType()
    {
        OpenResponseType(ResponseEventType.Crutch);
    }

    public void CloseCrutchResponseType()
    {
        CloseResponseType(ResponseEventType.Crutch);
    }

    public override DialogueObject GetDialogue()
    {
        Hotbar hotbar = Hotbar.Instance;
        if (!getCrutch)
        {
            if (hotbar.mainItem != null && hotbar.mainItem == crutchItem)
            {
                return getCrutchDialogue;
            }
            else
            {
                return waitForCrutchDialogue;
            }
        }
        else
        {
            return endingDialogue;
        }
    }
}
