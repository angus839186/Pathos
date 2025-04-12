using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musician : InteractableNPC, IDataPersistence
{
    public bool gotMusicScore;
    public Item musicScoreItem;
    public DialogueObject normalDialogue;
    public DialogueObject TakeMusicScoreDialogue;


    public override DialogueObject GetDialogue()
    {
        if (!gotMusicScore)
        {
            if (CheckItemOnPlayer(musicScoreItem) != null)
            {
                return TakeMusicScoreDialogue;
            }
            else
            {
                return normalDialogue;
            }
        }
        else
        {
            return normalDialogue;
        }
    }

    public void LoadData(GameData data)
    {
        gotMusicScore = data.MusicianGotMusicScore;
    }

    public void SaveData(ref GameData data)
    {
        data.MusicianGotMusicScore = gotMusicScore;
    }
}
