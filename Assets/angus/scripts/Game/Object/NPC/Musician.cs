using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musician : InteractableNPC, IDataPersistence
{
    public bool gotMusicScore;
    public Item musicScoreItem;
    public DialogueObject normalDialogue;
    public DialogueObject TakeMusicScoreDialogue;

    public AudioSource MusicianAudio;

    public Animator anime;

    void Start()
    {
        anime = GetComponent<Animator>();
        MusicianAudio = GetComponent<AudioSource>();
    }


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
    public void PlaySong()
    {
        StartCoroutine(Singing());
    }

    public IEnumerator Singing()
    {
        MusicianAudio.Play();
        anime.SetBool("singing", true);
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        while (MusicianAudio.isPlaying)
        {
            yield return null;
        }
        collider.enabled = true;
        anime.SetBool("singing", false);
    }
}
