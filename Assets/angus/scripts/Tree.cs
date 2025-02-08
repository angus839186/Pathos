using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    // 指定砍樹所需要的道具（例如：斧頭）
    public Item axeItem;
    public bool isCutDown = false;

    public string DefaultDescription = "巨大的枯木，鳥兒常會停在這裡休息";
    public string CutDownDescription = "被砍倒的枯木，不知道鳥兒去哪兒了";

    public string GetDescription()
    {
        return isCutDown ? CutDownDescription : DefaultDescription;
    }

    public void Interact(Item heldItem)
    {
        // 若樹木已砍倒，僅顯示敘述，不進行其他動作
        if (isCutDown)
        {
            Debug.Log(CutDownDescription);
            return;
        }

        // 若樹木尚未砍倒且玩家持有斧頭，就執行砍樹動作
        if (heldItem != null && heldItem == axeItem)
        {
            Debug.Log("玩家使用斧頭砍樹！");
            // 取得 Animator 播放砍樹動畫（此處需在樹上附加 Animator 組件）
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Chop");
            }
            // 直接改變狀態；你也可以在動畫事件中設置 isCutDown = true
            isCutDown = true;
        }
        else
        {
            // 未持有斧頭時，只顯示原始敘述（例如提示玩家無法進行砍樹）
            Debug.Log(DefaultDescription);
        }
    }
}
