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

    public bool isActive;

    public Rigidbody2D playerRB;
    public float moveSpeed;
    public float jumpForce;
    private float velocity;
    public float jumpHangTime;
    private float currentJumpHangTime;

    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    private bool isGrounded;

    public float knockback;
    public bool knockbackFromRight;
    public float knockbackLength;
    public float knockbackCounter;
    public float knockbackCooldownLength;
    public float knockbackCooldownCounter;

    public Animator playerAnimator;

    public Transform attackPoint;
    public GameObject axeProjectile;
    private bool attackPressed;
    public float swooshAttackCooldown;
    public float axeAttackCooldown;
    private float attackCounter;

    public float deathTime = 3f;
    private float deathTimeCounter;
    public GameObject playerdeathEffect;

    public string triggerObject;
    private bool lockTriggerUsing = false;
    public int keysCount;

    public EquipmentManager equipmentManager;

    public CollectablesController collectableController;
    public CompleteLevelScreen completeLevelScreen;
    public Button buttonBackToMap;
    private float timeInLevel;
    public Text currentTime;

    private bool isDataLoaded = false;
    private bool isLevelLoaded = false;
    private bool isLevelCompleted = false;

    public bool isBossEncounter = false;

    public ParticleSystem footDust;
    private ParticleSystem.EmissionModule footDustEmission;
    public ParticleSystem jumpDust;
    private bool wasGrounded;

    #endregion

    void Start()
    {
        isActive = true;
        GameManager.instance.AddPlayer(this);

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

        footDustEmission = footDust.emission;
        jumpDust.gameObject.SetActive(false);
    }

    private void LateUpdate() //Load
    {
        if (!isDataLoaded)
        {
            GameManager.LoadJsonData(GameManager.instance);
            equipmentManager.UpdateEquipment();
            isDataLoaded = true;
        }
    }

    void Update()
    {
        if (isActive)
        {
            CheckGround();
            MoveAndKnockback();
            AttackCooldown();
            Animate();
            UpdateCompleteLevelScreenTimer();

            if(isGrounded)
            {
                currentJumpHangTime = jumpHangTime;
            }
            else
            {
                currentJumpHangTime -= Time.deltaTime;
            }

            if (attackPressed && !isLevelCompleted)
            {
                if (attackCounter <= 0)
                {
                    Attack();
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

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);
    }

    private void MoveAndKnockback()
    {
        if (knockbackCounter <= 0)
        {
            playerRB.velocity = new Vector2(velocity * moveSpeed, playerRB.velocity.y);
        }
        else
        {    
            if (knockbackFromRight)
            {
                playerRB.velocity = new Vector2(-knockback, knockback / 2);
            }
            else
            {
                playerRB.velocity = new Vector2(knockback, knockback / 2);
            }
            
            knockbackCounter -= Time.deltaTime;
        }

        knockbackCooldownCounter -= Time.deltaTime;
    }

    private void Animate()
    {
        playerAnimator.SetBool(Consts.IS_GROUNDED, isGrounded);
        playerAnimator.SetFloat(Consts.SPEED, Mathf.Abs(playerRB.velocity.x));
        playerAnimator.SetFloat(Consts.YSPEED, playerRB.velocity.y);
        if (playerRB.velocity.x < 0f)
        {
            playerRB.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (playerRB.velocity.x > 0f)
        {
            playerRB.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if(playerRB.velocity.x != 0f && isGrounded)
        {
            footDustEmission.rateOverTime = 35f;
        }
        else
        {
            footDustEmission.rateOverTime = 0f;
        }

        if (!wasGrounded && isGrounded)
        {
            jumpDust.gameObject.SetActive(true);
            jumpDust.Stop();
            jumpDust.transform.position = footDust.transform.position;
            jumpDust.Play();
        }

        wasGrounded = isGrounded;
    }

    private void Attack()
    {
        switch (equipmentManager.currentItem)
        {
            case (int)EquipmentManager.Items.Swoosh:
                playerAnimator.SetTrigger(Consts.ATTACK);
                attackCounter = swooshAttackCooldown;
                break;
            case (int)EquipmentManager.Items.Axe:
                Instantiate(axeProjectile, attackPoint.position, attackPoint.rotation);
                attackCounter = axeAttackCooldown;
                break;
            default:
                break;
        }
    }

    private void AttackCooldown()
    {
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
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
                GotoLevel("LevelMap");
            }
            else if (triggerObject.Equals("CompleteLevel"))
            {
                ShowCompleteLevelScreen();          
            }
            else if (triggerObject.StartsWith("Level"))
            {
                GameManager.levelMapLastPosition = playerRB.transform.position;
                GotoLevel(triggerObject);          
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

    public void GotoLevel(string levelName)
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
            StartCoroutine(GameManager.instance.LoadLevel(levelName));
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
        GotoLevel("LevelMap");
    }

    public void Death()
    {
        isActive = false;
        velocity = 0;

        Instantiate(playerdeathEffect, transform.position, transform.rotation);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;

        deathTimeCounter = deathTime;
    }

    public void Respawn()
    {
        if (!isBossEncounter)
        {
            GotoLevel(SceneManager.GetActiveScene().name);
        }
        else
        {
            isActive = true;
            isBossEncounter = false;

            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CapsuleCollider2D>().enabled = true;

            playerRB.velocity = new Vector3(0, 0, 0);

            transform.position = GameManager.instance.currentCheckPoint.transform.position;

            var hpController = GetComponentInChildren<PlayerHPController>();
            hpController.currentHP = hpController.maxHP;

            GameManager.instance.PlayerRespawnEffect();
        }
    }

    public void CollectKey()
    {
        keysCount++;
    }

    public void CollectWeapon(string weapon)
    {
        Debug.Log($"znalezles now¹ broñ {weapon}");

        var objectsAfterBoss = GameObject.Find("ObjectsAfterBoss");

        if (objectsAfterBoss != null)
        {
            if (weapon.Equals(Consts.AXE_WEAPON_COLLECTABLE))
            {
                objectsAfterBoss.gameObject.transform.position = new Vector3(-6.85f, 25.0f, 0f);
            }
        }

        GameManager.levelList[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)] = true;
        GameManager.SaveJsonData(GameManager.instance);

        equipmentManager.UpdateEquipment();
    }

    #region Touch control

    public void JumpOnTouch()
    {
        if (isActive && !isLevelCompleted)
        {
            if (currentJumpHangTime > 0f)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }
        }
    }

    public void JumpStopOnTouch()
    {
        if (isActive && !isLevelCompleted)
        {
            if (!isGrounded && playerRB.velocity.y > 0f)
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

    public void ChangeItemOnTouch()
    {
        if (isActive)
        {
            equipmentManager.ChangeItem();
        }
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

    #region Keyboard control

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
            if (context.started && currentJumpHangTime > 0f)
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

    public void AttackKeyboard(InputAction.CallbackContext context)
    {
        if (isActive && !isLevelCompleted)
        {
            if (context.started && attackCounter <= 0)
            {
                Attack();
            }
        }
    }

    #endregion
}
