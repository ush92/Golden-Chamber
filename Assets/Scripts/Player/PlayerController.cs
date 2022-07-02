using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region variables

    private bool isLoaded = false;
    public bool isAlive;

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

    public float deathTime = 3f;
    private float deathTimeCounter;
    public GameObject playerdeathEffect;

    public string triggerObject;

    #endregion

    void Start()
    {
        GameManager.instance.AddPlayer(this);
        isAlive = true;
    }   

    void Update()
    {
        if (isAlive)
        {
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
    }

    private void LateUpdate() //Load
    {
        if (!isLoaded)
        {
            GameManager.LoadJsonData(GameManager.instance);
            isLoaded = true;
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
                LoadLevel(Consts.LEVEL1);
                break;

            case Consts.LEVEL_MAP:
                LoadLevel(Consts.LEVEL_MAP);
                break;

            default:
                Debug.Log("uzyto nieobslugiwany trigger");
                break;
        }
    }

    private static void LoadLevel(string levelName)
    {
        GameManager.SaveJsonData(GameManager.instance); //autosave
                                                        
        SceneManager.LoadScene(levelName);
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
}
