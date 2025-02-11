using UnityEngine;
using System.Collections.Generic;

public class gong : MonoBehaviour, IInteractable
{

    public Item axeItem;

    public string DefaultDescription = "很大的鑼，看起來徒手沒辦法敲響的樣子";

    public List<Bird> birds = new List<Bird>();

    public GameObject skyPos;

    public string GetDescription()
    {
        return DefaultDescription;
    }

    public string GetAnimationTrigger(Item heldItem)
    {
        if (heldItem == axeItem)
        {
            return "Chop";
        }
        return "";
    }

    public void Interact(Item heldItem)
    {

        if (heldItem != null && heldItem == axeItem)
        {
            Animator anime = GetComponent<Animator>();
            if (anime != null)
            {
                BirdFlyToSky();
            }
        }
        else
        {
            Debug.Log(DefaultDescription);
        }
    }
    public void BirdFlyToSky()
    {
        if (birds.Count < 5)
        {
            foreach (var bird in birds)
            {
                bird.FlyBack();
                break;
            }
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
            skyPos.SetActive(true);
        }
    }
}
