using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region variables

    public bool isAlive;
    public bool isKeyboard2;
    public GameObject player2;

    public Rigidbody2D playerRB;
    public float moveSpeed;
    public float jumpForce;
    private float velocity;

    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    private bool isGrounded;
    public GameObject stomper;

    public float knockback;
    public float knockbackLength;
    public bool knockbackFromRight;
    public float knockbackCounter;

    public Animator playerAnimator;

    public float attackCooldown = 0.25f;
    private float attackCounter;

    private string cameraSplitStyle;

    public float deathTime = 3f;
    private float deathTimeCounter;
    public GameObject playerdeathEffect;

    public string triggerObject;

    #endregion

    void Start()
    {
        GameManager.instance.AddPlayer(this);
        isAlive = true;

        SetDefaultCamera();
    }

    private void SetDefaultCamera()
    {
        if (GameManager.instance.activePlayers.Count == 1)
        {
            GameManager.instance.camera1.rect = new Rect(0f, 0f, 1f, 1f); ;
        }
        else if (GameManager.instance.activePlayers.Count == 2)
        {

            GameManager.instance.camera1.rect = new Rect(0, 0.5f, 1f, 0.5f);
            GameManager.instance.camera1.orthographicSize = 4f;

            GameManager.instance.camera2.rect = new Rect(0f, 0f, 1f, 0.5f);
            GameManager.instance.camera2.orthographicSize = 4f;

            GameManager.instance.camera2.GetComponent<SmoothFollow>().target = GameManager.instance.activePlayers[1].transform;
            cameraSplitStyle = Consts.HORIZONTAL;
        }
    }

    void Update()
    {
        if (isAlive)
        {
            if (isKeyboard2)
            {
                Player2Control();
            }

            CheckGround();
            Knockback();
            AttackCooldown();
            Animate();
        }
        else
        {
            if (deathTimeCounter > 0)
            {
                deathTimeCounter -= Time.deltaTime;
            }
            else
            {
                Respawn();
                deathTimeCounter = deathTime;
            }
        }

        if (GameManager.onePlayerMode == false && GameManager.instance.activePlayers.Count < 2)
        {
            player2.SetActive(true);
            //Instantiate(player2, GameManager.instance.currentCheckPoint.transform.position, transform.rotation);           
        }

        SwitchCamera();
    }

    private void Player2Control()
    {
        velocity = 0f;

        if (Keyboard.current.lKey.isPressed)
        {
            velocity += 1f;
        }
        if (Keyboard.current.jKey.isPressed)
        {
            velocity -= 1f;
        }
        if (isGrounded && Keyboard.current.rightShiftKey.wasPressedThisFrame)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }
        if (!isGrounded && Keyboard.current.rightShiftKey.wasReleasedThisFrame && playerRB.velocity.y > 0)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * 0.5f);
        }
        if (Keyboard.current.enterKey.wasPressedThisFrame && attackCounter <= 0)
        {
            playerAnimator.SetTrigger(Consts.ATTACK);
            attackCounter = attackCooldown;
        }
        if (Keyboard.current.iKey.isPressed && isGrounded)
        {
            GameManager.instance.camera2.GetComponent<SmoothFollow>().isLookingUp = true;
        }
        if (Keyboard.current.iKey.wasReleasedThisFrame)
        {
            GameManager.instance.camera2.GetComponent<SmoothFollow>().isLookingUp = false;
        }
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            UseTrigger();
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);
    }

    private void Knockback()
    {
        if (knockbackCounter <= 0)
        {
            playerRB.velocity = new Vector2(velocity * moveSpeed, playerRB.velocity.y);
        }
        else
        {
            if (knockbackFromRight)
            {
                playerRB.velocity = new Vector2(-knockback, knockback);
            }
            else
            {
                playerRB.velocity = new Vector2(knockback, knockback);
            }
            knockbackCounter -= Time.deltaTime;
        }
    }

    private void AttackCooldown()
    {
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
    }

    private void Animate()
    {
        playerAnimator.SetBool(Consts.IS_GROUNDED, isGrounded);
        playerAnimator.SetFloat(Consts.SPEED, Mathf.Abs(playerRB.velocity.x));
        playerAnimator.SetFloat(Consts.YSPEED, playerRB.velocity.y);
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
        if (isAlive)
        {
            velocity = context.ReadValue<Vector2>().x;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isAlive)
        {
            if (context.started && isGrounded)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }

            if (!isGrounded && context.canceled && playerRB.velocity.y > 0)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * 0.5f);
            }
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (isAlive)
        {
            if (isGrounded)
            {
                GameManager.instance.camera1.GetComponent<SmoothFollow>().isLookingUp = !context.canceled;
            }  
        }
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (isAlive)
        {
            UseTrigger();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (isAlive)
        {
            if (context.started && attackCounter <= 0)
            {
                playerAnimator.SetTrigger(Consts.ATTACK);
                attackCounter = attackCooldown;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(Consts.MOVING_PLATFORM))
        {
            transform.parent = other.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(Consts.MOVING_PLATFORM))
        {
            transform.parent = null;
        }
    }

    private void UseTrigger()
    {
        switch (triggerObject)
        {
            case "":
                break;

            case Consts.LEVEL1:
                if(GameManager.onePlayerMode)
                {
                    SceneManager.LoadScene(Consts.LEVEL1);
                }
                else 
                {
                    if(GameManager.instance.activePlayers[0].triggerObject == GameManager.instance.activePlayers[1].triggerObject)
                    {
                        SceneManager.LoadScene(Consts.LEVEL1);
                    }
                    else
                    {
                        Debug.Log("Obaj gracze musz¹ stac na drzwiach zeby sie przeteleportowac");
                    }
                }
                break;

            case Consts.LEVEL2:
                Debug.Log(this.name + " used level 2 door");
                break;

            default:
                break;
        }
    }

    public void Death()
    {
        isAlive = false;
        velocity = 0;

        Instantiate(playerdeathEffect, transform.position, transform.rotation);

        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        stomper.gameObject.SetActive(false);

        deathTimeCounter = deathTime;
    }

    public void Respawn()
    {
        isAlive = true;

        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CapsuleCollider2D>().enabled = true;
        stomper.gameObject.SetActive(true);

        playerRB.velocity = new Vector3(0, 0, 0);

        transform.position = GameManager.instance.currentCheckPoint.transform.position;

        var hpController = this.GetComponentInChildren<PlayerHPController>();
        hpController.currentHP = hpController.maxHP;

        GameManager.instance.PlayerRespawnEffect();
    }

    public void SwitchCamera()
    {
        if (GameManager.instance.activePlayers.Count == 2 && Keyboard.current.f2Key.wasPressedThisFrame)
        {
            if (cameraSplitStyle == Consts.HORIZONTAL)
            {
                GameManager.instance.camera1.rect = new Rect(0, 0f, 0.5f, 1f);
                GameManager.instance.camera1.orthographicSize = 8f;
                GameManager.instance.camera2.rect = new Rect(0.5f, 0f, 0.5f, 1f);
                GameManager.instance.camera2.orthographicSize = 8f;
                cameraSplitStyle = Consts.VERTICAL;
            }
            else if (cameraSplitStyle == Consts.VERTICAL)
            {
                GameManager.instance.camera1.rect = new Rect(0, 0.5f, 1f, 0.5f);
                GameManager.instance.camera1.orthographicSize = 4f;
                GameManager.instance.camera2.rect = new Rect(0f, 0f, 1f, 0.5f);
                GameManager.instance.camera2.orthographicSize = 4f;
                cameraSplitStyle = Consts.HORIZONTAL;
            }
        }
    }
}
