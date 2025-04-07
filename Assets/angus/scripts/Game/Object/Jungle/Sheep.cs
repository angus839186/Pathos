using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : InteractableObject, IDataPersistence
{

    [SerializeField] private string id;

    [ContextMenu("Generate sheep id")]
    private void GenerateSheepID()
    {
        id = System.Guid.NewGuid().ToString();
    }
    [SerializeField] private bool colored;

    public float minX;
    public float maxX;

    public float moveSpeed;

    public string axeDescription;

    public Item axeItem;

    public Item coloredItem;

    [SerializeField] private Animator anime;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        anime = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(RandomWalk());
    }

    public override string GetDescription(Item heldItem)
    {
        if (heldItem == axeItem)
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
        if (hotbar.mainItem != null || hotbar.mainItem != coloredItem) return;
        GetColored();

    }

    public override void InteractEvent(Item heldItem)
    {
        //Do nothing
    }

    void GetColored()
    {
        if (colored) return;
        colored = true;
        anime.SetBool("colored", colored);
    }

    private void FlipDirection(Vector2 direction)
    {
        if (direction.x < 0)
            spriteRenderer.flipX = false;
        else if (direction.x > 0)
            spriteRenderer.flipX = true;
    }
    public IEnumerator RandomWalk()
    {
        while (true)
        {
            Vector2 targetPos = new Vector2(Random.Range(minX, maxX), transform.position.y);

            anime.SetBool("isWalking", true);

            while ((targetPos - (Vector2)transform.position).sqrMagnitude > 0.1f * 0.1f)
            {
                Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
                FlipDirection(direction);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;
            anime.SetBool("isWalking", false);

            yield return new WaitForSeconds(2.5f);
        }
    }

    public void LoadData(GameData data)
    {
        data.sheepGotColored.TryGetValue(id, out colored);
        anime.SetBool("colored", colored);
    }

    public void SaveData(ref GameData data)
    {
        if (data.sheepGotColored.ContainsKey(id))
        {
            data.sheepGotColored.Remove(id);
        }
        data.sheepGotColored.Add(id, colored);
    }
}
