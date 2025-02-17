using UnityEngine;

public class Trees : MonoBehaviour, IInteractable
{

    public Item axeItem;
    public bool isCutDown = false;

    public string DefaultDescription = "巨大的枯木，鳥兒常會停在這裡休息";
    public string CutDownDescription = "被砍倒的枯木，不知道鳥兒去哪兒了";

    public Bird[] birds;

    public AudioClip sound;

    public string GetDescription()
    {
        return isCutDown ? CutDownDescription : DefaultDescription;
    }

    public string GetAnimationTrigger(Item heldItem)
    {
        if (!isCutDown && heldItem == axeItem)
        {
            return "Chop";
        }
        return "";
    }


    public void BirdFlyAway()
    {
        foreach (var bird in birds)
        {
            bird.FlyToNextPos(bird.fencePos);
        }
    }

    public void InteractEvent(Item heldItem)
    {
        if (isCutDown)
        {
            return;
        }
        else
        {
            if (heldItem != null && heldItem == axeItem)
            {
                Animator anime = GetComponent<Animator>();
                if (anime != null)
                {
                    anime.SetTrigger("Chop");
                    AudioManager.instance.PlaySound(sound);
                    isCutDown = true;
                }
                BirdFlyAway();
            }
            else
            {
                return;
            }
        }
    }

    public void Interact()
    {
        Debug.Log(DefaultDescription);
    }
}
