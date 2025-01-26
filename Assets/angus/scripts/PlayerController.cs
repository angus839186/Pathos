using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputControl input;

    public Rigidbody2D rb;

    void Start()
    {
        input = GetComponent<InputControl>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveInput = input.move;

        Move(moveInput);
    }
    void Move(Vector2 moveDirection)
    {
        rb.velocity = moveDirection * 5;

    }
}
