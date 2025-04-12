using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageHeader : InteractableNPC, IDataPersistence
{
    public Item crutchItem;

    public bool getCrutch;

    public DialogueObject waitForCrutchDialogue;

    public DialogueObject getCrutchDialogue;

    public DialogueObject endingDialogue;

    public override DialogueObject GetDialogue()
    {
        if (!getCrutch)
        {
            if (CheckItemOnPlayer(crutchItem) != null)
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

    public void LoadData(GameData data)
    {
        getCrutch = data.VillageHeaderGetCrutch;
    }

    public void SaveData(ref GameData data)
    {
        data.VillageHeaderGetCrutch = getCrutch;
    }
}
