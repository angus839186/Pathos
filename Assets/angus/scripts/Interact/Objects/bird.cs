using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour, IDataPersistence
{

    [SerializeField] private string id;

    [ContextMenu("Generate bird id")]
    private void GenerateBirdID()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public Transform fencePos;

    public Transform skyPos;

    public float flightSpeed;
    public float landDistance = 0.5f;

    public bool isFlying;

    private Animator animator;

    public bool onFence;

    public bool flyToSky;

    public bool OriginOnFence;

    private SpriteRenderer spriteRenderer;

    public gong _gong;

    public AudioClip sound;



    void Start()
    {
        animator = GetComponent<Animator>();
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
        if (isFlying)
            return;
        StartCoroutine(FlyRoutine(nextPos));
    }
    private IEnumerator FlyRoutine(Transform nextPos)
    {

        isFlying = true;
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
        flyToSky = true;
        gameObject.SetActive(false);
    }

    public void FlyBack()
    {
        if (isFlying)
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

    public void LoadData(GameData data)
    {
        if (!OriginOnFence)
        {
            data.birdsFlied.TryGetValue(id, out onFence);
        }
        data.bridsOnFence.TryGetValue(id, out flyToSky);

        if (flyToSky)
        {
            Destroy(gameObject);
        }
        else
        {
            if (onFence)
            {
                gameObject.transform.position = fencePos.position;
            }
            else
            {
                return;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.birdsFlied.ContainsKey(id))
        {
            data.birdsFlied.Remove(id);
        }
        data.birdsFlied.Add(id, flyToSky);

        if (data.bridsOnFence.ContainsKey(id))
        {
            data.bridsOnFence.Remove(id);
        }
        data.bridsOnFence.Add(id, onFence);
    }
}
