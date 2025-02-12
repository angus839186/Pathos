using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windmill : MonoBehaviour, IInteractable
{
    public Animator animator;

    public string DefaultDescription;
    public string workedDescription;

    public bool worked;

    public OrganDoor door;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void windmillfailed()
    {
        animator.Play("windmill_fail", -1, 0f);
    }
    public void windmillworked()
    {
        animator.Play("windmill_work", -1, 0f);
        worked = true;
    }

    public void OpenOrganDoor()
    {
        door.Open();
    }

    public string GetDescription()
    {
        return worked ? workedDescription : DefaultDescription;
    }

    public string GetAnimationTrigger(Item heldItem)
    {
        return "";
    }

    public void Interact()
    {
        Debug.Log(DefaultDescription);
    }

    public void InteractEvent(Item heldItem)
    {
        throw new System.NotImplementedException();
    }
}
