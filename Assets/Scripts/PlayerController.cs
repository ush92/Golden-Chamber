using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public float moveSpeed;
    public float jumpForce;
    private float velocity;

    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator playerAnimator;

    public bool isKeyboard2;

    public float attackCooldown = 0.25f;
    private float attackCounter;

    void Start()
    {
        GameManager.instance.AddPlayer(this);

        if (GameManager.instance.activePlayers.Count == 1)
        {       
            this.GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 1); ;
        }
        else if (GameManager.instance.activePlayers.Count == 2)
        {
            this.GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1); ;
        }
    }

    void Update()
    {
        if(isKeyboard2)
        {
            velocity = 0f;

            if(Keyboard.current.lKey.isPressed)
            {
                velocity += 1f;
            }
            if (Keyboard.current.jKey.isPressed)
            {
                velocity -= 1f;
            }
            if(isGrounded && Keyboard.current.rightShiftKey.wasPressedThisFrame)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }
            if(!isGrounded && Keyboard.current.rightShiftKey.wasReleasedThisFrame && playerRB.velocity.y > 0)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * 0.5f);
            }
            if (Keyboard.current.enterKey.wasPressedThisFrame && attackCounter <= 0)
            {
                playerAnimator.SetTrigger("attack");
                attackCounter = attackCooldown;
            }
        }

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);
        playerRB.velocity = new Vector2(velocity * moveSpeed, playerRB.velocity.y);

        if(attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
            //playerRB.velocity = new Vector2(0f, playerRB.velocity.y);
        }

        Animate();
    }

    private void Animate()
    {
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("speed", Mathf.Abs(playerRB.velocity.x));
        playerAnimator.SetFloat("ySpeed", playerRB.velocity.y);
        if (playerRB.velocity.x < 0)
        {
            playerRB.transform.localScale = new Vector3(-1f, 1f, 1f);
        } 
        else if (playerRB.velocity.x > 0)
        {
            playerRB.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        velocity = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }

        if(!isGrounded && context.canceled && playerRB.velocity.y > 0)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * 0.5f);
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && attackCounter <= 0)
        {
            playerAnimator.SetTrigger("attack");
            attackCounter = attackCooldown;
        }
    }
}
