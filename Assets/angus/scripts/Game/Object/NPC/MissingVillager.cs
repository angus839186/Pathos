using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingVillager : InteractableNPC, IDataPersistence
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

    public void LoadData(GameData data)
    {
        giveCrutch = data.MissingVillagerGiveCrutch;
    }

    public void SaveData(ref GameData data)
    {
        data.MissingVillagerGiveCrutch = giveCrutch;
    }

    public void ToggleGiveCrutch(bool toggle)
    {
        giveCrutch = toggle;
    }

}
