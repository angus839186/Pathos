using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBusinessman : InteractableNPC, IDataPersistence
{
    public bool getBerry;
    public bool givePlayerScissorItem;

    public Item berryItem;
    public Item ScissorItem;

    public DialogueObject beforeGiveScissorDialogue;
    public DialogueObject afterGiveScissorDialogue;

    public DialogueObject TakeBerryDialogue;
    public DialogueObject afterTakeBerryDialogue;

    public override DialogueObject GetDialogue()
    {
        if (getBerry)
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

    public void LoadData(GameData data)
    {
        givePlayerScissorItem = data.ColorBusinessmanGivePlayerScissor;
        getBerry = data.ColorBusinessmanGetBerry;
    }

    public void SaveData(ref GameData data)
    {
        data.ColorBusinessmanGivePlayerScissor = givePlayerScissorItem;
        data.ColorBusinessmanGetBerry = getBerry;
    }

    public void ToggleGivePlayerScissor(bool toggle)
    {
        givePlayerScissorItem = toggle;
    }
    public void ToggleGotBerry(bool toggle)
    {
        getBerry = toggle;
    }
}
