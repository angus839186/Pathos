using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputControl : MonoBehaviour
{
    public Vector2 move;
    public void OnMove(InputValue value)
    {
        Debug.Log("move");
        MoveInput(value.Get<Vector2>());
    }

    public void MoveInput(Vector2 moveDirection)
    {
        move = moveDirection;
    }

}
