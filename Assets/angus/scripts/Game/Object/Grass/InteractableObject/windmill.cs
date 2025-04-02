using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windmill : InteractableObject, IDataPersistence
{
    public Animator animator;
    public string workedDescription;

    public bool worked;

    public OrganDoor door;

    public AudioClip failedSound;
    public AudioClip workedSound;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override string GetDescription()
    {
        return worked ? workedDescription : base.GetDescription();
    }

    public override string GetAnimationTrigger(Item heldItem)
    {
        return base.GetAnimationTrigger(null);
    }

    public override void Interact()
    {
        //Do Nothing
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do Nothing
    }
    public void windmillfailed()
    {
        animator.Play("windmill_fail", -1, 0f);
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(failedSound);
    }
    public void windmillworked()
    {
        animator.Play("windmill_work", -1, 0f);
        worked = true;
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = workedSound;
        audio.Stop();
        audio.Play();
        audio.loop = true;
        OpenOrganDoor();
    }

    public void OpenOrganDoor()
    {
        door.Open();
    }

    public void LoadData(GameData data)
    {
        worked = data.windmillWorked;
        if (worked)
        {
            windmillworked();
        }
    }

    public void SaveData(ref GameData data)
    {
        data.windmillWorked = worked;
    }
}
