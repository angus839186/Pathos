using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windmill : MonoBehaviour, IInteractable
{
    public Animator animator;

    public string DefaultDescription;
    public string workedDescription;

    public bool worked;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void windmillfailed()
    {
        animator.Play("windmill_fail");
    }
    public void windmillworked()
    {
        animator.Play("windmill_work");
        worked = true;
    }

    public void Interact(Item heldItem)
    {
        Debug.Log(DefaultDescription);
    }

    public string GetDescription()
    {
        return worked ? workedDescription : DefaultDescription;
    }

    public string GetAnimationTrigger(Item heldItem)
    {
        return "";
    }
}
