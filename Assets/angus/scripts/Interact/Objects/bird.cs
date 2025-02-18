using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Transform fencePos;

    public Transform skyPos;

    public float flightSpeed;
    public float landDistance = 0.5f;

    public bool isFlying;

    private Animator animator;

    public bool onFence;

    private SpriteRenderer spriteRenderer;

    public gong _gong;

    public AudioClip sound;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = this.transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FlipDirection(Vector2 direction)
    {
        if (direction.x < 0) // 往左
            spriteRenderer.flipX = true;
        else if (direction.x > 0) // 往右
            spriteRenderer.flipX = false;
    }

    public void FlyToNextPos(Transform nextPos)
    {
        if(isFlying)
        return;
        StartCoroutine(FlyRoutine(nextPos));
    }
    private IEnumerator FlyRoutine(Transform nextPos)
    {

        isFlying = true;
        Debug.Log("Start Flying");
        Vector2 direction = nextPos.position - transform.position;
        FlipDirection(direction);

        animator.Play("bird_takesoff");
        yield return new WaitForSeconds(0.5f);

        animator.Play("bird_fly");
        AudioManager.instance.PlaySound(sound);

        while (Vector2.Distance(transform.position, nextPos.position) > landDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, nextPos.position, flightSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        animator.Play("bird_landing");

        yield return new WaitForSeconds(0.5f);
        animator.Play("bird_wait");
        transform.position = nextPos.position;
        isFlying = false;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fence")
        {
            onFence = true;
            if (!_gong.birds.Contains(this))
            {
                _gong.birds.Add(this);
            }
            onFence = true;
        }
        if (collision.tag == "skyPos")
        {
            BirdsGone();
        }
    }
    void BirdsGone()
    {
        gameObject.SetActive(false);
    }

    public void FlyBack()
    {
        if(isFlying)
        return;
        StartCoroutine(FlyBackRoutine(fencePos));
    }

    private IEnumerator FlyBackRoutine(Transform fencePos)
    {
        isFlying = true;
        float Height = 15f;
        Vector3 randomOffset = new Vector2(Random.Range(-8f, 8f), Height);
        Vector3 randomTarget = transform.position + randomOffset;

        // 播放起飛動畫
        animator.Play("bird_takesoff");
        yield return new WaitForSeconds(0.5f);

        // 切換到飛行動畫
        animator.Play("bird_fly");

        // 飛往隨機的上方目標
        while (Vector2.Distance(transform.position, randomTarget) > landDistance)
        {
            Vector3 direction = (randomTarget - transform.position).normalized;
            FlipDirection(direction);
            transform.position = Vector2.MoveTowards(transform.position, randomTarget, flightSpeed * Time.deltaTime);
            yield return null;
        }

        // 在上方等待幾秒
        yield return new WaitForSeconds(2f);

        // 飛回 fencePos
        animator.Play("bird_fly");
        while (Vector2.Distance(transform.position, fencePos.position) > landDistance)
        {
            Vector3 direction = (fencePos.position - transform.position).normalized;
            FlipDirection(direction);
            transform.position = Vector2.MoveTowards(transform.position, fencePos.position, flightSpeed * Time.deltaTime);
            yield return null;
        }

        // 播放降落動畫
        yield return new WaitForSeconds(0.2f);
        animator.Play("bird_landing");
        yield return new WaitForSeconds(0.5f);
        animator.Play("bird_wait");
        transform.position = fencePos.position;
        isFlying = false;
    }
}
