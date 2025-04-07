using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBusinessman : InteractableNPC
{
    public bool gotBerry;
    public bool givePlayerScissorItem;

    public Item berryItem;
    public Item ScissorItem;

    public DialogueObject beforeGiveScissorDialogue;
    public DialogueObject afterGiveScissorDialogue;

    public DialogueObject TakeBerryDialogue;
    public DialogueObject afterTakeBerryDialogue;

    public override DialogueObject GetDialogue()
    {
        if (gotBerry)
        {
            return afterTakeBerryDialogue;
        }
        else
        {
            if (!givePlayerScissorItem)
            {
                return beforeGiveScissorDialogue;
            }
            else
            {
                if (CheckItemOnPlayer(berryItem) != null)
                {
                    return TakeBerryDialogue;
                }
                else
                {
                    return afterGiveScissorDialogue;
                }
            }
        }
    }

    public void ToggleGivePlayerScissor(bool toggle)
    {
        givePlayerScissorItem = toggle;
    }
    public void ToggleGotBerry(bool toggle)
    {
        gotBerry = toggle;
    }
}
