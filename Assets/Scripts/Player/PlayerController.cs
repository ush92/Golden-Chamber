using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region variables

    public bool isActive;

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

    private bool attackPressed;
    public float attackCooldown = 0.25f;
    private float attackCounter;

    public float deathTime = 3f;
    private float deathTimeCounter;
    public GameObject playerdeathEffect;

    public string triggerObject;
    private bool lockTriggerUsing = false;
    public int keysCount;

    public CollectablesController collectableController;
    public CompleteLevelScreen completeLevelScreen;
    public Button buttonBackToMap;
    private float timeInLevel;
    public Text currentTime;

    private bool isDataLoaded = false;
    private bool isLevelLoaded = false;
    private bool isLevelCompleted = false;

    #endregion

    void Start()
    {
        GameManager.instance.AddPlayer(this);
        isActive = true;

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
        if (isActive)
        {
            CheckGround();
            Knockback();
            AttackCooldown();
            Animate();
            UpdateCompleteLevelScreenTimer();

            if (attackPressed && !isLevelCompleted)
            {
                if (attackCounter <= 0)
                {
                    playerAnimator.SetTrigger(Consts.ATTACK);
                    attackCounter = attackCooldown;
                }
            }
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

    private void UpdateCompleteLevelScreenTimer()
    {
        if (!isLevelCompleted && !SceneManager.GetActiveScene().name.Equals(Consts.LEVEL_MAP))
        {
            timeInLevel += Time.deltaTime;

            var timeInMinutes = (int)timeInLevel / 60;
            var timeInSeconds = (int)timeInLevel % 60;
            currentTime.text = timeInMinutes.ToString() + " min " + timeInSeconds.ToString() + "s";

            int record = GameManager.levelRecords[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)][9];

            if ((int)timeInLevel > record && record != 0)
            {
                currentTime.color = new Color32(220, 20, 60, 255);
            }
            else if ((int)timeInLevel == record)
            {
                currentTime.color = new Color32(182, 182, 182, 255);
            }
            else
            {
                currentTime.color = new Color32(60, 179, 113, 255);
            }
        }
    }

    private void LateUpdate() //Load
    {
        if (!isDataLoaded)
        {
            GameManager.LoadJsonData(GameManager.instance);
            isDataLoaded = true;
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
        if (isActive && !isLevelCompleted)
        {
            velocity = context.ReadValue<Vector2>().x;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isActive && !isLevelCompleted)
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
        if (isActive && !isLevelCompleted)
        {
            if (isGrounded)
            {
                GameManager.instance.playerCamera.GetComponent<SmoothFollow>().isLookingUp = !context.canceled;
            }
        }
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (isActive && !isLevelCompleted)
        {
            UseTrigger();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (isActive && !isLevelCompleted)
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
        else if (other.gameObject.name.Equals(Consts.LOCKED_DOOR_P))
        {
            if (keysCount > 0)
            {
                keysCount--;

                foreach (var c in other.gameObject.GetComponents<BoxCollider2D>())
                {
                    c.enabled = false;
                }

                other.transform.Find(Consts.LOCKED_DOOR).gameObject.SetActive(false);
                other.transform.Find(Consts.OPENED_DOOR).gameObject.SetActive(true);
            }
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
        if (!lockTriggerUsing)
        {
            lockTriggerUsing = true;

            if (triggerObject.Equals("LevelMap"))
            {
                LoadLevel("LevelMap");
            }
            else if (triggerObject.StartsWith("Complete_"))
            {
                ShowCompleteLevelScreen();          
            }
            else if (triggerObject.StartsWith("Level"))
            {
                GameManager.levelMapLastPosition = playerRB.transform.position;
                LoadLevel(triggerObject);          
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
    }

    public void LoadLevel(string levelName)
    {
        if (!isLevelLoaded)
        {
            isLevelLoaded = true;

            if (levelName.Equals(Consts.LEVEL_MAP))
            {
                buttonBackToMap.interactable = false;
                buttonBackToMap.GetComponent<EventTrigger>().enabled = false;
            }

            GameManager.SaveJsonData(GameManager.instance);
            SceneManager.LoadScene(levelName);
        }
    }

    private void ShowCompleteLevelScreen()
    {      
        GameManager.levelList[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)] = true;

        isLevelCompleted = true;
        completeLevelScreen.gameObject.SetActive(true);

        collectableController.UpdateLevelFruitRecord(Consts.GetLevelIndex(SceneManager.GetActiveScene().name));
        completeLevelScreen.UpdateLevelTimeRecord(Consts.GetLevelIndex(SceneManager.GetActiveScene().name), (int)timeInLevel);

        GameManager.SaveJsonData(GameManager.instance);       
    }

    private void FinishLevelAndLoadMap()
    {
        completeLevelScreen.gameObject.SetActive(false);
        LoadLevel("LevelMap");
    }

    public void Death()
    {
        isActive = false;
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
        //isAlive = true;

        //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //GetComponent<SpriteRenderer>().enabled = true;
        //GetComponent<CapsuleCollider2D>().enabled = true;
        ////stomper.gameObject.SetActive(true);

        //playerRB.velocity = new Vector3(0, 0, 0);

        //transform.position = GameManager.instance.currentCheckPoint.transform.position;

        //var hpController = this.GetComponentInChildren<PlayerHPController>();
        //hpController.currentHP = hpController.maxHP;

        //GameManager.instance.PlayerRespawnEffect();

        LoadLevel(SceneManager.GetActiveScene().name); //reload level
    }

    public void CollectKey()
    {
        keysCount++;
    }

    #region Touch control

    public void JumpOnTouch()
    {
        if (isActive && !isLevelCompleted)
        {
            if (isGrounded)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }
        }
    }
    public void JumpStopOnTouch()
    {
        if (isActive && !isLevelCompleted)
        {
            if (!isGrounded && playerRB.velocity.y > 0)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * 0.5f);
            }
        }
    }
    public void AttackButtonDownOnTouch()
    {
        attackPressed = true;
    }
    public void AttackButtonUpOnTouch()
    {
        attackPressed = false;
    }

    public void LeftOnTouch()
    {
        if (isActive && !isLevelCompleted)
        {
            velocity -= 1f;
        }
    }
    public void RightOnTouch()
    {
        if (isActive && !isLevelCompleted)
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
        if (isActive && !isLevelCompleted)
        {
            UseTrigger();
        }
    }
    public void LookOnTouch()
    {
        if (isActive && !isLevelCompleted && isGrounded)
        {
            GameManager.instance.playerCamera.GetComponent<SmoothFollow>().isLookingUp = true;
        }
    }
    public void LookStopOnTouch()
    {
        if (isActive && !isLevelCompleted && isGrounded)
        {
            GameManager.instance.playerCamera.GetComponent<SmoothFollow>().isLookingUp = false;
        }
    }

    #endregion

}
