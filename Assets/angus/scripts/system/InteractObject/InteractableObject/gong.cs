using UnityEngine;
using System.Collections.Generic;

public class gong : InteractableObject
{
    public Item axeItem;
    public string birdsGoneDescription;
    public List<Bird> birds = new List<Bird>();

    public GameObject skyPos;
    public windmill windmill;
    public AudioClip sound;
    public bool birdsGone;

    public override string GetDescription()
    {
        return birdsGone ? birdsGoneDescription : base.GetDescription();
    }

    public override string GetAnimationTrigger(Item heldItem)
    {
        return heldItem == axeItem ? "Chop" : base.GetAnimationTrigger(null);
    }

    public override void Interact()
    {
        //Do Nothing
    }

    public override void InteractEvent(Item heldItem)
    {
        if (heldItem != null && heldItem == axeItem)
        {
            Animator anime = GetComponent<Animator>();
            anime.Play("gong_anime", -1, 0f);
            AudioManager.instance.PlaySound(sound);
            if (!windmill.worked)
            {
                BirdFlyToSky();
            }
        }
    }

    public void BirdFlyToSky()
    {
        if (birds.Count < 5)
        {
            foreach (var bird in birds)
            {
                bird.FlyBack();
            }
            windmill.Invoke("windmillfailed", 1f);
            skyPos.SetActive(false);
            return;
        }

        bool allOnFence = true;
        foreach (var bird in birds)
        {
            if (!bird.onFence)
            {
                allOnFence = false;
                break;
            }
        }

        if (allOnFence)
        {
            foreach (var bird in birds)
            {
                bird.FlyToNextPos(bird.skyPos);
            }
            windmill.Invoke("windmillworked", 1f);
            skyPos.SetActive(true);
            birdsGone = true;
        }
    }
}

