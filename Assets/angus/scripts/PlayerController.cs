using System.Collections;
using System.Collections.Generic;
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

    public Animator _anime;

    public SpriteRenderer _sprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _anime = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
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
    public void OnMove(InputValue inputValue)
    {
        moveVector = inputValue.Get<Vector2>();
    }

    public void OnJump(InputValue inputValue)
    {
        float jumpInput = inputValue.Get<float>();
        if (jumpInput > 0 && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGrounded = false;
            _anime.SetTrigger("jumpTrigger");
            _anime.SetBool("isJumping", isGrounded);
            Debug.Log("執行跳躍");
        }
    }
    public void OnRun(InputValue inputValue)
    {
        isRunning = inputValue.isPressed;
        _anime.SetBool("runKey", isRunning);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 當碰撞到標記為 "Ground" 的物件時，重設 isGrounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _anime.SetBool("isJumping", isGrounded);
            Debug.Log("已落地");
        }
    }
}
