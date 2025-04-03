using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : InteractableObject
{
    [SerializeField] private string axeDescription;
    [SerializeField] private Animator berryTreeAnime;
    [SerializeField] private GameObject cutAnimationObject;

    [SerializeField] private Item berryItem;

    [SerializeField] private Item ScissorsItem;

    [SerializeField] private Item axeItem;

    void Start()
    {
        berryTreeAnime = GetComponent<Animator>();
    }

    public override string GetDescription(Item heldItem)
    {
        if(heldItem == axeItem)
        {
            return axeDescription;
        }
        else
        {
            return base.GetDescription(null);
        }
    }
    public override void Interact()
    {
        Hotbar hotbar = Hotbar.Instance;
        if (hotbar.mainItem == null || hotbar.mainItem.item != ScissorsItem)return;
        StartCoroutine(playCutAnimation());
    }

    public override void InteractEvent(Item heldItem)
    {
        //Do nothing
    }

    public IEnumerator playCutAnimation()
    {
        cutAnimationObject.SetActive(true);
        Animator anime = cutAnimationObject.GetComponent<Animator>();
        anime.SetTrigger("cut");
        PlayerInteraction playerInteract = FindObjectOfType<PlayerInteraction>();
        playerInteract.ToggleInteractingAnimation(true);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = anime.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("cat_cut") && stateInfo.normalizedTime >= 1f;
        });
        playerInteract.ToggleInteractingAnimation(false);
        cutAnimationObject.SetActive(false);
        berryTreeAnime.SetTrigger("cut");
        InventoryManager.Instance.AddItem(berryItem);
        yield return null;
    }

}
