using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : InteractableObject
{
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
        if (direction.x < 0) // 往左
            spriteRenderer.flipX = false;
        else if (direction.x > 0) // 往右
            spriteRenderer.flipX = true;
    }
    public IEnumerator RandomWalk()
    {
        while (true)
        {
            // 在迴圈內重新生成隨機目標位置（絕對位置）
            Vector2 targetPos = new Vector2(Random.Range(minX, maxX), transform.position.y);

            anime.SetBool("isWalking", true);

            // 當物件與目標的距離大於一定範圍時持續移動
            while ((targetPos - (Vector2)transform.position).sqrMagnitude > 0.1f * 0.1f)
            {
                // 每次移動時根據最新位置計算方向
                Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
                FlipDirection(direction);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 確保到達精準的目標位置
            transform.position = targetPos;
            anime.SetBool("isWalking", false);

            yield return new WaitForSeconds(2.5f);
        }
    }
}
