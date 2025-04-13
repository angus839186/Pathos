using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletinBoard : InteractableNPC, IDataPersistence
{
    public bool sheepMissionCompleted;

    public GameObject BulletinBoardText;

    public DialogueObject firstDialogue;
    public DialogueObject secondDialogue;
    public override DialogueObject GetDialogue()
    {
        if(!sheepMissionCompleted)
        {
            return firstDialogue;
        }
        else
        {
            return secondDialogue;
        }
    }
    public void LoadData(GameData data)
    {
        sheepMissionCompleted = data.ShepherdAlreadyKnowSheepColored;
        BulletinBoardText.SetActive(!sheepMissionCompleted);
    }

    public void SaveData(ref GameData data)
    {
        //Do nothing
    }
}
