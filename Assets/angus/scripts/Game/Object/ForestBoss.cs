using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBoss : InteractableNPC
{
    public Animator anime;
    public void playAttackAnime()
    {
        anime.SetTrigger("Attack");
    }
    public void playDieAnime()
    {
        anime.SetTrigger("Die");
    }
}
