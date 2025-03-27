using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public Rigidbody2D rb;
    public Vector2 moveVector;

    public bool firstLoad;

    [Header("跳躍高度")]
    public float jumpPower;

    [Header("移動速度")]
    public int speed;

    [Header("跑步速度倍率")]
    public float runMultiplier = 1.5f;

    public bool isRunning;
    public bool isGrounded;
    public bool canMove = false;

    public Animator _anime;
    public SpriteRenderer _sprite;

    [Header("音效")]
    public AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip jumpSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _anime = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        PlayerInputManager.Instance.OnMoveEvent += HandleMove;
        PlayerInputManager.Instance.OnJumpEvent += HandleJump;
        PlayerInputManager.Instance.OnRunEvent += HandleRun;
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnMoveEvent -= HandleMove;
        PlayerInputManager.Instance.OnJumpEvent -= HandleJump;
        PlayerInputManager.Instance.OnRunEvent -= HandleRun;
    }

    void Update()
    {
        _anime.SetFloat("yVelocity", rb.velocity.y);
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            _anime.SetBool("isWalking", false);
            _anime.SetBool("runKey", false);
            return;
        }
        else
        {
            float currentSpeed = isRunning ? speed * runMultiplier : speed;
            if (moveVector != Vector2.zero)
            {
                rb.velocity = new Vector2(moveVector.x * currentSpeed, rb.velocity.y);
                _anime.SetBool("runKey", isRunning);
                _anime.SetBool("isWalking", true);
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = isRunning ? runSound : walkSound;
                    audioSource.Play();
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                _anime.SetBool("isWalking", false);
                audioSource.Stop();
            }
        }

        if (moveVector.x > 0)
            _sprite.flipX = false;
        else if (moveVector.x < 0)
            _sprite.flipX = true;
    }

    // 處理移動輸入
    private void HandleMove(Vector2 move)
    {
        if (!canMove)
        {
            moveVector = Vector2.zero;
            return;
        }
        moveVector = move;
    }

    // 處理跳躍輸入
    private void HandleJump(float jump)
    {
        if (!canMove) return;

        if (jump > 0 && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGrounded = false;
            _anime.SetTrigger("jumpTrigger");
            _anime.SetBool("isJumping", true);
            AudioManager.instance.PlaySound(jumpSound);
        }
    }

    // 處理跑步輸入
    private void HandleRun(bool run)
    {
        isRunning = run;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _anime.SetBool("isJumping", false);
        }
        FireBall fireBall = collision.gameObject.GetComponent<FireBall>();
        if (fireBall != null)
        {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(1);
        }
    }

    public void LoadData(GameData data)
    {
        if (!firstLoad)
        {
            if (data.playerPosition == Vector3.zero)
            {
                transform.position = GameObject.Find("SpawnPoint").transform.position;
                firstLoad = true;
            }
            else
            {
                transform.position = data.playerPosition;
                firstLoad = true;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }
}
