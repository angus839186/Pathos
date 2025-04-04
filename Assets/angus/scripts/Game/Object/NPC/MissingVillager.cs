using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingVillager : InteractableNPC
{
    public bool giveCrutch;

    public DialogueObject giveCrutchDialogue;

    public DialogueObject afterGiveCrutchDialogue;

    public override DialogueObject GetDialogue()
    {
        if (!giveCrutch)
        {
            return giveCrutchDialogue;
        }
        else
        {
            return afterGiveCrutchDialogue;
        }
    }

    public void ToggleGiveCrutch(bool toggle)
    {
        giveCrutch = toggle;
    }

}
