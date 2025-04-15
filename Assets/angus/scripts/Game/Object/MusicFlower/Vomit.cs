using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vomit : InteractableObject
{
    private int cleanIndex;

    [SerializeField] private Sprite[] vomitSprites;

    [SerializeField] SpriteRenderer vomitSprite;

    public AudioClip vomitSound;

    private int MaxCleanIndex = 3;

    [SerializeField] private GameObject missingVillager;

    void Start()
    {
        vomitSprite = GetComponent<SpriteRenderer>();
        vomitSprite.sprite = vomitSprites[cleanIndex];
    }
    public override string GetAnimationTrigger(Item heldItem)
    {
        return "clean";
    }
    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    public override void InteractEvent(Item heldItem)
    {
        cleanVomit();
    }
    void cleanVomit()
    {
        cleanIndex++;
        AudioManager.instance.PlaySound(vomitSound);
        if(cleanIndex >= MaxCleanIndex)
        {
            missingVillager.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            vomitSprite.sprite = vomitSprites[cleanIndex];
        }
    }
}
