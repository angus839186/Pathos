using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public Rigidbody2D rb;
    public Vector2 moveVector;
    public Vector2 swimVector;

    public bool firstLoad;

    [Header("跳躍高度")]
    public float jumpPower;

    [Header("移動速度")]
    public int speed;

    [Header("跑步速度倍率")]
    public float runMultiplier = 1.5f;

    private bool isRunning;
    private bool isGrounded;
    public bool canMove = true;

    public Animator _anime;
    public SpriteRenderer _sprite;

    [Header("音效")]
    public AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip jumpSound;

    [Header("游泳")]
    public bool isSwimming;
    public float swimSpeed = 3f;
    public float waterDrag = 2f;
    public float waterGravityScale = 0f;

    public float defaultDrag;
    public float defaultGravityScale;

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
        PlayerInputManager.Instance.OnSwimEvent += HandleSwim;
        PlayerInputManager.Instance.OnJumpEvent += HandleJump;
        PlayerInputManager.Instance.OnRunEvent += HandleRun;
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnMoveEvent -= HandleMove;
        PlayerInputManager.Instance.OnSwimEvent -= HandleSwim;

        PlayerInputManager.Instance.OnJumpEvent -= HandleJump;
        PlayerInputManager.Instance.OnRunEvent -= HandleRun;
    }

    void Update()
    {
        if (!isSwimming)
        {
            _anime.SetFloat("yVelocity", rb.velocity.y);
        }
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
            if (isSwimming)
            {
                if (swimVector != Vector2.zero)
                {
                    Vector2 targetVel = swimVector.normalized * swimSpeed;
                    rb.velocity = Vector2.Lerp(rb.velocity, targetVel, waterDrag * Time.fixedDeltaTime);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
                _anime.SetBool("swimVelocity", swimVector != Vector2.zero);

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
                    _anime.SetBool("runKey", isRunning);
                    audioSource.Stop();
                }
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

    private void HandleSwim(Vector2 move)
    {
        if (!canMove)
        {
            swimVector = Vector2.zero;
            return;
        }
        swimVector = move;
    }

    // 處理跳躍輸入
    private void HandleJump(float jump)
    {
        if (!canMove) return;

        if (isSwimming) return;
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
        if (!isSwimming)
        {
            isRunning = run;
        }
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

    public void ToggleMove(bool toggle)
    {
        canMove = toggle;
        if (!toggle)
        {
            rb.velocity = Vector2.zero;
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
        if (SceneManager.GetActiveScene().name == "forest_Reverse")
        {
            isSwimming = true;
            rb.gravityScale = waterGravityScale;
            rb.drag = waterDrag;
        }
        else
        {
            isSwimming = false;
            rb.gravityScale = defaultGravityScale;
            rb.drag = defaultDrag;
        }
        _anime.SetBool("isSwimming", isSwimming);
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }
}
