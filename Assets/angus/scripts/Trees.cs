using UnityEngine;

public class Trees : MonoBehaviour, IInteractable
{
    // 指定砍樹所需要的道具（例如：斧頭）
    public Item axeItem;
    public bool isCutDown = false;

    public string DefaultDescription = "巨大的枯木，鳥兒常會停在這裡休息";
    public string CutDownDescription = "被砍倒的枯木，不知道鳥兒去哪兒了";

    public Bird[] birds;

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

    public void Interact(Item heldItem)
    {
        // 樹已倒，不進行互動動作
        if (isCutDown)
        {
            Debug.Log(CutDownDescription);
            return;
        }

        if (heldItem != null && heldItem == axeItem)
        {
            Debug.Log("玩家使用斧頭砍樹！");
            Animator anime = GetComponent<Animator>();
            if (anime != null)
            {
                anime.SetTrigger("Chop");
            }
            // 可以在動畫事件中更改 isCutDown 狀態
            isCutDown = true;
        }
        else
        {
            Debug.Log(DefaultDescription);
        }
    }
    public void BirdFlyAway()
    {
        foreach (var bird in birds)
        {
            bird.FlyToNextPos();
        }
    }
}
