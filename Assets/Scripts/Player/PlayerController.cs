using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    #region variables

    public bool isActive;

    public Rigidbody2D playerRB;
    public float baseMoveSpeed;
    private float moveSpeed;
    public float waterMoveSpeed;
    private float velocity;

    public float jumpForce;
    public float jumpHangTime;
    private float currentJumpHangTime;

    private float oneWayTileCooldown = 0.2f;
    public float oneWayTileCounter;

    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    private bool isGrounded;

    public float knockback;
    public bool knockbackFromRight;
    public float knockbackLength;
    public float knockbackCounter;
    public float knockbackCooldownLength;
    public float knockbackCooldownCounter;

    public bool isSwimming = false;

    public Animator playerAnimator;

    private bool attackPressed;
    public Transform attackPoint;
    private float attackCounter;
    public float swooshAttackCooldown;
    public GameObject axeProjectile;
    public float axeAttackCooldown;
    public GameObject stoneProjectile;
    public float stoneAttackCooldown;
    public GameObject fireSparkProjectile;
    public float fireSparkAttackCooldown;

    public float deathTime = 3f;
    private float deathTimeCounter;
    public GameObject playerdeathEffect;

    public string triggerObject;
    private bool lockTriggerUsing = false;
    public int keysCount;

    public CollectablesController collectableController;
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

    //UI
    public EquipmentManager equipmentManager;
    public GameObject soundOptionsWindow;
    public CompleteLevelScreen completeLevelScreen;
    public Button buttonBackToMap;
    public GameObject pauseWindow;

    //Specific levels
    //2_3
    private bool isHp5ItemCollected = false;
    private bool isStoneWeaponCollected = false;

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

        moveSpeed = baseMoveSpeed;

        footDustEmission = footDust.emission;
        jumpDust.gameObject.SetActive(false);

        DisablePlayerUI();
    }

    private void LateUpdate() //Load
    {
        if (!isDataLoaded)
        {
            isDataLoaded = true;
            GameManager.LoadJsonData(GameManager.instance);
            equipmentManager.currentItem = PlayerPrefs.GetInt(Consts.PLAYER_CURRENT_ITEM);
            equipmentManager.UpdateEquipment();
        }
    }

    void Update()
    {
        if (isActive)
        {
            if (isSwimming) Swimming();
            CheckGround();
            MoveAndKnockback();
            AttackCooldown();
            Animate();
            UpdateCompleteLevelScreenTimer();

            if (isGrounded)
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
            var timeRestInSeconds = (int)timeInLevel % 60;
            currentTime.text = timeInMinutes.ToString() + " min " + timeRestInSeconds.ToString() + "s";

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
        if (!isSwimming)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);
        }
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

            if (knockbackCounter > 0)
            {
                knockbackCounter -= Time.deltaTime;
            }
        }

        if (knockbackCooldownCounter > 0)
        {
            knockbackCooldownCounter -= Time.deltaTime;
        }

        if(oneWayTileCounter > 0)
        {
            oneWayTileCounter -= Time.deltaTime;
        }
    }

    private void Animate()
    {
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

        if (!isSwimming)
        {
            playerAnimator.SetBool(Consts.IS_GROUNDED, isGrounded);

            if (playerRB.velocity.x != 0f && isGrounded)
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
        else
        {
            playerAnimator.SetBool(Consts.IS_SWIMMING, true);

            if (playerRB.velocity.y > 0)
            {
                playerAnimator.SetBool(Consts.IS_GROUNDED, false);
            }
            else
            {
                playerAnimator.SetBool(Consts.IS_GROUNDED, true);
            }
        }
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
                playerAnimator.SetTrigger(Consts.RANGED_ATTACK);
                Instantiate(axeProjectile, attackPoint.position, attackPoint.rotation);
                attackCounter = axeAttackCooldown;
                break;
            case (int)EquipmentManager.Items.Stone:
                playerAnimator.SetTrigger(Consts.RANGED_ATTACK);
                Instantiate(stoneProjectile, attackPoint.position, attackPoint.rotation);
                attackCounter = stoneAttackCooldown;
                break;
            case (int)EquipmentManager.Items.FireSpark:
                playerAnimator.SetTrigger(Consts.RANGED_ATTACK);
                Instantiate(fireSparkProjectile, attackPoint.position, attackPoint.rotation);
                attackCounter = fireSparkAttackCooldown;
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

    private void Swimming()
    {
        playerRB.gravityScale = 0.2f;
        isGrounded = true;
        jumpForce = 3;
        moveSpeed = baseMoveSpeed / 2;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(Consts.MOVING_PLATFORM))
        {
            transform.parent = other.transform;
        }

        if (other.gameObject.tag.Equals(Consts.ONE_WAY_TILE))
        {
            triggerObject = Consts.ONE_WAY_TILE;
        }

        if (other.gameObject.name.Equals(Consts.LOCKED_DOOR_P))
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

    private void OnCollisionStay2D(Collision2D other) 
    {
        if (other.gameObject.name.Equals(Consts.SLIDER_RIGHT))
        {
            playerRB.AddForce(Vector2.right * 300, ForceMode2D.Force);
        }
        else if (other.gameObject.name.Equals(Consts.SLIDER_LEFT))
        {
            playerRB.AddForce(Vector2.left * 300, ForceMode2D.Force);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(Consts.MOVING_PLATFORM))
        {
            transform.parent = null;
        }

        if (other.gameObject.tag.Equals(Consts.ONE_WAY_TILE))
        {
            triggerObject = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.StartsWith(Consts.WATER_TOP) ||
            other.gameObject.name.StartsWith(Consts.WATER_BOT))
        {
            moveSpeed = waterMoveSpeed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.StartsWith(Consts.WATER_TOP) ||
            other.gameObject.name.StartsWith(Consts.WATER_BOT))
        {
            moveSpeed = waterMoveSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.StartsWith(Consts.WATER_TOP) ||
            other.gameObject.name.StartsWith(Consts.WATER_BOT))
        {
            moveSpeed = baseMoveSpeed;
        }
    }

    private void UseTriggeredObject()
    {
        if (!lockTriggerUsing)
        {
            lockTriggerUsing = true;

            if (triggerObject.Equals(Consts.LEVEL_MAP))
            {
                DisablePlayerUI();
                GotoLevel(Consts.LEVEL_MAP);
            }
            else if (triggerObject.Equals(Consts.COMPLETE_LEVEL))
            {
                DisablePlayerUI();
                ShowCompleteLevelScreen();          
            }
            else if (triggerObject.StartsWith(Consts.LEVEL))
            {
                DisablePlayerUI();
                GameManager.levelMapLastPosition = playerRB.transform.position;
                GotoLevel(triggerObject);          
            }
            else if(triggerObject.Equals(Consts.ONE_WAY_TILE))
            {
                if(oneWayTileCounter <= 0)
                {
                    oneWayTileCounter = oneWayTileCooldown;
                }

                lockTriggerUsing = false;
            }
            else
            {
                switch (triggerObject)
                {
                    default:
                        lockTriggerUsing = false;
                        Debug.Log("Uzyto nieobslugiwany trigger");
                        break;
                }
            }
        }
    }

    private void DisablePlayerUI()
    {
        soundOptionsWindow.gameObject.SetActive(false);

        //disable summaryWindow as well
    }

    public void GotoLevel(string levelName)
    {
        if (!isLevelLoaded)
        {
            PlayerPrefs.SetInt(Consts.PLAYER_CURRENT_ITEM, equipmentManager.currentItem);

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
        try
        {
            GameManager.levelList[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)] = true;

            isLevelCompleted = true;
            completeLevelScreen.gameObject.SetActive(true);

            collectableController.UpdateLevelFruitRecord(Consts.GetLevelIndex(SceneManager.GetActiveScene().name));
            completeLevelScreen.UpdateLevelTimeRecord(Consts.GetLevelIndex(SceneManager.GetActiveScene().name), (int)timeInLevel);

            GameManager.SaveJsonData(GameManager.instance);
        }
        catch (Exception e)
        {
            Debug.Log($"ShowCompleteLevelScreen error with exception {e.Message}");
        }
    }

    private void FinishLevelAndLoadMap()
    {
        completeLevelScreen.gameObject.SetActive(false);
        GotoLevel(Consts.LEVEL_MAP);
    }

    public void Death()
    {
        isActive = false;
        velocity = 0;

        Instantiate(playerdeathEffect, transform.position, transform.rotation);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        footDustEmission.rateOverTime = 0f;

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

    public void CollectBossItem(string weapon)
    {
        var objectsAfterBoss = GameObject.Find(Consts.OBJECTS_AFTER_BOSS);

        if (objectsAfterBoss != null)
        {
            switch(weapon)
            {
                case Consts.AXE_WEAPON_COLLECTABLE:
                    objectsAfterBoss.gameObject.transform.position = new Vector3(-6.85f, 25.0f, 0f);
                    break;
                case Consts.STONE_WEAPON_COLLECTABLE:
                    isStoneWeaponCollected = true;
                    break;
                case Consts.HP_MAX_PLUS_5:
                    isHp5ItemCollected = true;
                    break;
                case Consts.FIRESPARK_WEAPON_COLLECTABLE:
                    objectsAfterBoss.gameObject.transform.position = new Vector3(104.522f, 5.61f, 0f);
                    break;
                default:
                    Debug.Log($"nieznana nazwa przedmiotu z bossa: {weapon}");
                    break;
            }      
        }

        if(isHp5ItemCollected && isStoneWeaponCollected)
        {
            objectsAfterBoss.gameObject.transform.position = new Vector3(-27.53f, 3.65f, 0f);
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
            UseTriggeredObject();
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
            UseTriggeredObject();
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
