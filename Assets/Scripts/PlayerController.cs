using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public float moveSpeed;
    public float jumpForce;
    public float velocity;

    void Start()
    {
        
    }

    void Update()
    {
        playerRB.velocity = new Vector2(velocity * moveSpeed, playerRB.velocity.y);
       
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        velocity = context.ReadValue<Vector2>().x;
    }
}
