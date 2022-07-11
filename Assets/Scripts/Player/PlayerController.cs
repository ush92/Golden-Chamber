using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    //public GameObject stomper;

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
    private bool lockTriggerUsing = false;

    public Button buttonBackToMap;
    public GameObject levelCompleteScreen;
    private bool isLevelCompleted;
    public float timeInLevel;

    #endregion

    void Start()
    {
        GameManager.instance.AddPlayer(this);
        isAlive = true;

        if (SceneManager.GetActiveScene().name.Equals(Consts.LEVEL_MAP))
        {
            buttonBackToMap.GetComponent<EventTrigger>().enabled = false;
            buttonBackToMap.interactable = false;

            if (!GameManager.levelMapLastPosition.Equals(float3.zero))
            {
                playerRB.transform.position = GameManager.levelMapLastPosition;
            }
        }
        else
        {
            buttonBackToMap.interactable = true;
        }
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

        if (!isLevelCompleted)
        {
            timeInLevel += Time.deltaTime;
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
        if (isAlive && !isLevelCompleted)
        {
            velocity = context.ReadValue<Vector2>().x;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isAlive && !isLevelCompleted)
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
        if (isAlive && !isLevelCompleted)
        {
            if (isGrounded)
            {
                GameManager.instance.playerCamera.GetComponent<SmoothFollow>().isLookingUp = !context.canceled;
            }
        }
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (isAlive && !isLevelCompleted)
        {
            UseTrigger();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (isAlive && !isLevelCompleted)
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
        if(triggerObject.Equals("LevelMap"))
        {
            if (!lockTriggerUsing)
            {
                LoadLevel("LevelMap");
                lockTriggerUsing = true;
            }
        }
        else if (triggerObject.StartsWith("Complete_"))
        {
            if (!lockTriggerUsing)
            {
                try
                {
                    int levelNum = int.Parse(triggerObject.Substring(triggerObject.LastIndexOf('_') + 1));
                    GameManager.levelList[levelNum] = true;
                    lockTriggerUsing = true;

                    ShowCompleteLevelScreen();
                }
                catch
                {
                    Debug.Log($"Drzwi ukonczenia rundy zawieraja z³a nazwe: {triggerObject}. Powinna konczyc siê odpowiednim indexem listy leveli");
                }
            }
        }
        else if (triggerObject.StartsWith("Level"))
        {
            if (!lockTriggerUsing)
            {
                GameManager.levelMapLastPosition = playerRB.transform.position;
                LoadLevel(triggerObject);
                lockTriggerUsing = true;
            }
        }
        else
        {
            switch (triggerObject)
            {
                default:
                    Debug.Log("Uzyto nieobslugiwany trigger");
                    break;
            }
        }
    }

    public void LoadLevel(string levelName)
    {
        if (levelName.Equals(Consts.LEVEL_MAP))
        {
            buttonBackToMap.interactable = false;
            buttonBackToMap.GetComponent<EventTrigger>().enabled = false;
        }

        GameManager.SaveJsonData(GameManager.instance);
        SceneManager.LoadScene(levelName);
    }

    private void ShowCompleteLevelScreen()
    {
        isLevelCompleted = true;      
        levelCompleteScreen.gameObject.SetActive(true);
    }

    private void FinishLevelAndLoadMap()
    {
        levelCompleteScreen.gameObject.SetActive(false);
        LoadLevel("LevelMap");
    }

    public void Death()
    {
        isAlive = false;
        velocity = 0;

        Instantiate(playerdeathEffect, transform.position, transform.rotation);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        //stomper.gameObject.SetActive(false);

        deathTimeCounter = deathTime;
    }

    public void Respawn()
    {
        isAlive = true;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = true;
        //stomper.gameObject.SetActive(true);

        playerRB.velocity = new Vector3(0, 0, 0);

        transform.position = GameManager.instance.currentCheckPoint.transform.position;

        var hpController = this.GetComponentInChildren<PlayerHPController>();
        hpController.currentHP = hpController.maxHP;

        GameManager.instance.PlayerRespawnEffect();
    }

    #region Touch control

    public void JumpOnTouch()
    {
        if (isAlive && !isLevelCompleted)
        {
            if (isGrounded)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }
        }
    }
    public void JumpStopOnTouch()
    {
        if (isAlive && !isLevelCompleted)
        {
            if (!isGrounded && playerRB.velocity.y > 0)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * 0.5f);
            }
        }
    }
    public void FireOnTouch()
    {
        if (isAlive && !isLevelCompleted)
        {
            if (attackCounter <= 0)
            {
                playerAnimator.SetTrigger(Consts.ATTACK);
                attackCounter = attackCooldown;
            }
        }
    }
    public void LeftOnTouch()
    {
        if (isAlive && !isLevelCompleted)
        {
            velocity -= 1f;
        }
    }
    public void RightOnTouch()
    {
        if (isAlive && !isLevelCompleted)
        {
            velocity += 1f;
        }
    }
    public void MoveStopTouch()
    {
        velocity = 0;
    }
    public void UseOnTouch()
    {
        if (isAlive && !isLevelCompleted)
        {
            UseTrigger();
        }
    }
    public void LookOnTouch()
    {
        if (isAlive && !isLevelCompleted && isGrounded)
        {
            GameManager.instance.playerCamera.GetComponent<SmoothFollow>().isLookingUp = true;
        }
    }
    public void LookStopOnTouch()
    {
        if (isAlive && !isLevelCompleted && isGrounded)
        {
            GameManager.instance.playerCamera.GetComponent<SmoothFollow>().isLookingUp = false;
        }
    }

    #endregion

}
