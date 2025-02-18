using UnityEngine;
using System.Collections.Generic;

public class gong : MonoBehaviour, IInteractable
{

    public Item axeItem;

    public string DefaultDescription;
    public string birdsGoneDescription;

    public List<Bird> birds = new List<Bird>();

    public GameObject skyPos;

    public windmill windmill;

    public AudioClip sound;

    public bool birdsGone;

    public string GetDescription()
    {
        return birdsGone ? birdsGoneDescription : DefaultDescription;
    }

    public string GetAnimationTrigger(Item heldItem)
    {
        if (heldItem == axeItem)
        {
            return "Chop";
        }
        return "";
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
        // 先檢查所有鳥的 onFence 是否都是 true
        bool allOnFence = true;
        foreach (var bird in birds)
        {
            if (!bird.onFence)
            {
                allOnFence = false;
                break;
            }
        }

        // 如果所有鳥都在 fence 上，才讓鳥飛走
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

    public void Interact()
    {
        Debug.Log(DefaultDescription);
    }

    public void InteractEvent(Item heldItem)
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
}
