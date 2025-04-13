using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : InteractableNPC
{
    public Item pendantItem;

    public DialogueObject GetPendantDialogue;
    public DialogueObject NormalDialogue;

    public override DialogueObject GetDialogue()
    {
        if(CheckItemOnPlayer(pendantItem) != null)
        {
            return GetPendantDialogue;
        }
        else
        {
            return NormalDialogue;
        }
    }
}
