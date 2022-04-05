using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public float moveSpeed;
    public float jumpForce;

    void Start()
    {
        
    }

    void Update()
    {
        playerRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerRB.velocity.y);

        if(Input.GetButtonDown("Jump"))
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }
    }
}
