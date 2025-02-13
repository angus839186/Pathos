using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 moveVector;
    
    [Header("跳躍高度")]
    public float jumpPower;
    
    [Header("移動速度")]
    public int speed;
    
    [Header("跑步速度倍率")]
    public float runMultiplier = 1.5f;
    
    public bool isRunning;
    public bool isGrounded;
    public bool isInteracting = false;
    
    public Animator _anime;
    public SpriteRenderer _sprite;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _anime = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }
    
    void OnEnable()
    {
        // 訂閱 InputManager 的事件
        PlayerInputManager.Instance.OnMoveEvent += HandleMove;
        PlayerInputManager.Instance.OnJumpEvent += HandleJump;
        PlayerInputManager.Instance.OnRunEvent += HandleRun;
    }
    
    void OnDisable()
    {
        // 訂閱要取消
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
        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        if (moveVector != Vector2.zero)
        {
            rb.velocity = new Vector2(moveVector.x * currentSpeed, rb.velocity.y);
            _anime.SetBool("isWalking", true);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            _anime.SetBool("isWalking", false);
        }
        
        if (moveVector.x > 0)
        {
            _sprite.flipX = false;
        }
        else if (moveVector.x < 0)
        {
            _sprite.flipX = true;
        }
    }
    
    // 處理移動輸入
    private void HandleMove(Vector2 move)
    {
        if (isInteracting)
        {
            moveVector = Vector2.zero;
            return;
        }
        moveVector = move;
    }
    
    // 處理跳躍輸入
    private void HandleJump(float jump)
    {
        if (isInteracting) return;
        
        if (jump > 0 && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGrounded = false;
            _anime.SetTrigger("jumpTrigger");
            _anime.SetBool("isJumping", true);
        }
    }
    
    // 處理跑步輸入
    private void HandleRun(bool run)
    {
        isRunning = run;
        _anime.SetBool("runKey", isRunning);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _anime.SetBool("isJumping", false);
            Debug.Log("已落地");
        }
    }
}
