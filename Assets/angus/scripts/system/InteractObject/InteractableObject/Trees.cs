using System.Collections;
using UnityEngine;

public class Trees : InteractableObject, IDataPersistence
{
    public Item axeItem;
    [SerializeField] private string id;

    [ContextMenu("Generate tree id")]
    private void GenerateTreeID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public bool isCutDown = false;
    public string CutDownDescription = "被砍倒的枯木，不知道鳥兒去哪兒了";

    public Bird[] birds;
    public AudioClip sound;
    public Sprite treeFallSprite;
    public Animator anime;

    void Awake()
    {
        anime = GetComponent<Animator>();
    }

    public override string GetDescription()
    {
        return isCutDown ? CutDownDescription : base.GetDescription();
    }

    public override string GetAnimationTrigger(Item heldItem)
    {
        return heldItem == axeItem ? "Chop" : base.GetAnimationTrigger(null);
    }

    public override void Interact()
    {
        //Do Nothing
    }

    public override void InteractEvent(Item heldItem)
    {
        if (isCutDown) return;

        if (heldItem != null && heldItem == axeItem)
        {
            if (anime != null)
            {
                anime.SetTrigger("Chop");
                AudioManager.instance.PlaySound(sound);
                isCutDown = true;
                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                collider.enabled = false;
            }
            BirdFlyAway();
        }
    }

    public void BirdFlyAway()
    {
        StartCoroutine(BirdFlyAwayRoutine());
    }

    private IEnumerator BirdFlyAwayRoutine()
    {
        foreach (var bird in birds)
        {
            // 加入隨機延遲，例如 0 到 0.1 秒之間
            yield return new WaitForSeconds(Random.Range(0f, 0.1f));
            bird.FlyToNextPos(bird.fencePos);
        }
    }

    public void LoadData(GameData data)
    {
        data.treesFalled.TryGetValue(id, out isCutDown);
        if (isCutDown)
        {
            anime.enabled = false;
            SpriteRenderer treeSprite = GetComponent<SpriteRenderer>();
            treeSprite.sprite = treeFallSprite;
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.treesFalled.ContainsKey(id))
        {
            data.treesFalled.Remove(id);
        }
        data.treesFalled.Add(id, isCutDown);
    }
}

