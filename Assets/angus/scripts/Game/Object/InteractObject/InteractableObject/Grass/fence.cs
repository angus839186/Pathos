using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fence : InteractableObject
{
    public override string GetAnimationTrigger(Item heldItem)
    {
        return base.GetAnimationTrigger(null);
    }

    public override string GetDescription()
    {
        return base.GetDescription();
    }

    public override void Interact()
    {
        //Do Nothing
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do Nothing
    }
}
