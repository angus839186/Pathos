using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : InteractableNPC, IDataPersistence
{
    public bool seePendant;
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

    public void LoadData(GameData data)
    {
        seePendant = data.GateKeeperSeePendant;
    }

    public void SaveData(ref GameData data)
    {
        data.GateKeeperSeePendant = seePendant;
    }

    public void ToggleSeePendant(bool toggle)
    {
        seePendant = toggle;
    }
}
