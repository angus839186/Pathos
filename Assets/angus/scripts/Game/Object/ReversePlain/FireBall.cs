using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [HideInInspector]
    public bool IsExploded = false;   // 火球是否已進入爆炸狀態

    private Rigidbody2D rb;
    private Animator anime;

    private AudioSource audioSource;
    private float launchSpeed;        // 初始發射速度
    private float downwardSpeed;      // 下墜速度

    // 掉落範圍參數
    private float dropMinX;
    private float dropMaxX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 初始化火球參數：僅設定上射狀態
    /// </summary>
    /// <param name="launchSpeed">發射速度</param>
    /// <param name="downwardSpeed">下墜速度</param>
    /// <param name="dropMinX">掉落區域最小 X 值</param>
    /// <param name="dropMaxX">掉落區域最大 X 值</param>
    /// <param name="groundY">地面 Y 坐標</param>
    public void Initialize(float launchSpeed, float downwardSpeed, float dropMinX, float dropMaxX)
    {
        this.launchSpeed = launchSpeed;
        this.downwardSpeed = downwardSpeed;
        this.dropMinX = dropMinX;
        this.dropMaxX = dropMaxX;
        
        // 發射階段：向上發射 (0, launchSpeed)
        rb.velocity = new Vector2(0, launchSpeed);
        // 將火球旋轉設定為朝上 (假設 90° 為上)
        transform.rotation = Quaternion.Euler(0, 0, -180);
    }

    /// <summary>
    /// 由 Boss 通知火球開始下墜
    /// 下墜時會根據當前位置計算，讓火球落在指定掉落範圍內的某個隨機目標點，同時把角度調整為朝下
    /// </summary>
    public void StartFalling()
    {
        float targetX = Random.Range(dropMinX, dropMaxX);
        Vector2 currentPos = transform.position;
        
        transform.position = new Vector2(targetX, currentPos.y);
        rb.velocity = new Vector2(0, -downwardSpeed);
        
        // 轉換角度為朝下 (這裡設為 -90°，依你實際素材調整)
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // 當火球與地面或玩家碰撞時觸發爆炸
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            if (!IsExploded)
            {
                StartCoroutine(Explode());
            }
        }
    }

    // 爆炸處理：播放爆炸動畫與音效，並等待後銷毀物件
    IEnumerator Explode()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.enabled = false;
        IsExploded = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        PlayExplosion();

        // 假設爆炸動畫長度為 1 秒
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void PlayExplosion()
    {
        anime.Play("fireattack_1");
        audioSource.Play();
    }
}
