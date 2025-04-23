using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class savePoint : InteractableObject
{
    public override string GetAnimationTrigger(Item heldItem)
    {
        return base.GetAnimationTrigger(null);
    }

    public override string GetDescription(Item heldItem)
    {
        return base.GetDescription(null);
    }

    public override void Interact()
    {
        SaveFileMenu.Instance.ActivateSaveFileCanvas();
        PlayerInputManager.Instance.SwitchActionMap("SaveMenu");
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do nothing
    }
}
