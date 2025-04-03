using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class gong : InteractableObject
{
    public Item axeItem;
    public string birdsGoneDescription;
    public List<Bird> birds = new List<Bird>();

    public GameObject skyPos;
    public windmill windmill;
    public AudioClip sound;
    public bool birdsGone;

    private Animator anime;

    private void Awake()
    {
        anime = GetComponent<Animator>();
    }

    public override string GetDescription(Item heldItem)
    {
        return birdsGone ? birdsGoneDescription : base.GetDescription(null);
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
            StartCoroutine(BirdFlyBackRoutine());
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
    private IEnumerator BirdFlyBackRoutine()
    {
        foreach (var bird in birds)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.1f));
            bird.FlyBack();
        }
    }
}

