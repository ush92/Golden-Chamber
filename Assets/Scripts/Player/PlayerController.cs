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
    public float baseMoveSpeed;
    private float moveSpeed;
    public float velocity;

    public float basedJumpForce;
    private float jumpForce;
    public float jumpHangTime;
    private float currentJumpHangTime;

    private float oneWayTileCooldown = 0.5f;
    public float oneWayTileCounter;

    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    public bool isGrounded;

    public float knockback;
    public bool knockbackFromRight;
    public float knockbackLength;
    public float knockbackCounter;
    public float knockbackCooldownLength;
    public float knockbackCooldownCounter;

    public bool isSwimming = false;
    public bool isFrozen;
    public float frozenLength;
    public float frozenCounter;
    private bool isSliding;
    public float slidingLenght;
    private float slidingCounter;

    public Animator playerAnimator;
    public PlayerSounds playerSounds;

    public Transform attackPoint;
    private bool attackPressed;
    private bool meleeAttackPressed;
    private float attackCounter;
    private float meleeAttackCounter;
    public GameObject swooshAttack;
    public PlayerProjectile axeProjectile;
    public PlayerProjectile stoneProjectile;
    public PlayerProjectile fireSparkProjectile;
    public GameObject arcticBreathe;
    public PlayerProjectile poisonProjectile;
    public PlayerProjectile goldenAxeProjectile;

    public float deathTime = 3f;
    private float deathTimeCounter;
    public GameObject playerdeathEffect;

    public string triggerObject;
    private bool lockTriggerUsing = false;
    public int keysCount;

    public CollectablesController collectableController;
    private float timeInLevel;
    public Text currentTime;

    private bool isLevelLoaded = false;
    public bool isLevelCompleted = false;
    public bool isBossEncounter = false;

    public ParticleSystem footDust;
    public ParticleSystem jumpDust;
    public ParticleSystem frostBreatheParticles;
    public ParticleSystem waterBubblesParticles;
    private ParticleSystem.EmissionModule footDustEmission;
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
    //2_4
    public Image oxygenBorder;
    public Image oxygenBar;
    public float maxOxygen;
    private float currentOxygen;
    //4_1
    private bool isGoldenAxeCollected = false;
    private bool isEpicTreasureCollected = false;

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
        isSliding = false;
        isSwimming = false;

        arcticBreathe.SetActive(false); //weapon

        //particle effects
        footDustEmission = footDust.emission;
        jumpDust.gameObject.SetActive(false);      
        frostBreatheParticles.gameObject.SetActive(SceneManager.GetActiveScene().name.Equals(Consts.LEVEL3_2));
        waterBubblesParticles.gameObject.SetActive(SceneManager.GetActiveScene().name.Equals(Consts.LEVEL2_4));
        oxygenBorder.gameObject.SetActive(SceneManager.GetActiveScene().name.Equals(Consts.LEVEL2_4));
        footDust.gameObject.SetActive(!SceneManager.GetActiveScene().name.Equals(Consts.LEVEL2_4));

        if (SceneManager.GetActiveScene().name.Equals(Consts.LEVEL2_4))
        {
            isSwimming = true;
            currentOxygen = maxOxygen;
        }

        DisablePlayerUI();
    }

    void Update()
    {
        if (isActive)
        {
            CheckMoveSpeed();
            CheckGround();
            MoveAndKnockback();
            AttackCooldowns();
            Animate();
            UpdateCompleteLevelScreenTimer();

            if (attackPressed && !isLevelCompleted)
            {
                if (attackCounter <= 0 && meleeAttackCounter <= 0)
                {
                    Attack();
                }
            }

            if (meleeAttackPressed && !isLevelCompleted)
            {
                if (meleeAttackCounter <= 0)
                {
                    MeleeAttack();
                }
            }

            ArcticBreathe(); //weapon

            if (SceneManager.GetActiveScene().name.Equals(Consts.LEVEL2_4))
            {
                UnderwaterBreathe();
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

    private void FixedUpdate()
    {
        if (isActive)
        {
            CheckMoveSpeed();
            CheckGround();
            MoveAndKnockback();
        }
    }

    private void CheckMoveSpeed()
    {
        if (isSwimming)
        {
            isGrounded = true;
            playerRB.gravityScale = 0.2f;
            jumpForce = 3.0f;
            moveSpeed = baseMoveSpeed / 2.0f;
        }
        else
        {
            playerRB.gravityScale = 5.0f;

            if (isFrozen)
            {
                frozenCounter = frozenLength;
                isFrozen = false;
            }

            frozenCounter -= Time.deltaTime;

            if (frozenCounter <= 0)
            {
                frozenCounter = 0;
                jumpForce = basedJumpForce;
                moveSpeed = baseMoveSpeed;
                GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
                jumpForce = basedJumpForce * 0.875f;
                moveSpeed = baseMoveSpeed / 3.0f;
                GetComponent<SpriteRenderer>().color = new Color32(80, 135, 255, 255);
            }
        }
        
        if (isSliding)
        {
            slidingCounter -= Time.deltaTime;
        }
        if (slidingCounter <= 0)
        {
            isSliding = false;
            slidingCounter = slidingLenght;
        }
    }

    private void ArcticBreathe()
    {
        if (!arcticBreathe.activeInHierarchy)
        {
            if (equipmentManager.currentItem == (int)EquipmentManager.Items.ArcticBreathe && attackPressed && velocity == 0 && isGrounded && isActive && !isLevelCompleted)
            {
                playerAnimator.SetTrigger(Consts.RANGED_ATTACK);
                arcticBreathe.SetActive(true);

                if (!playerSounds.Arctic1.isPlaying)
                {
                    SoundEffect($"Arctic1", 1f, 1.5f);
                }
            }
        }
        else
        {
            if (equipmentManager.currentItem != (int)EquipmentManager.Items.ArcticBreathe || !attackPressed || velocity != 0 || !isGrounded || !isActive || isLevelCompleted)
            {
                arcticBreathe.SetActive(false);

                if (playerSounds.Arctic1.isPlaying)
                {
                    playerSounds.Arctic1.Stop();
                }
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

            if (isGrounded && playerRB.velocity.y < 0 && oneWayTileCounter <= 0) //fix velocity_Y
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0.0f);
            }
        }

        if (isGrounded)
        {
            currentJumpHangTime = jumpHangTime;
        }
        else
        {
            currentJumpHangTime -= Time.deltaTime;
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
                if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_3)] == false)
                {
                    swooshAttack.GetComponent<DamageEnemy>().damageToDeal = WeaponsConsts.SWOOSH_BASIC_DMG;
                    playerAnimator.SetTrigger(Consts.ATTACK);
                    meleeAttackCounter = WeaponsConsts.SWOOSH_BASIC_CD;
                    SoundEffect($"Swoosh{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
                }
                else
                {
                    swooshAttack.GetComponent<DamageEnemy>().damageToDeal = WeaponsConsts.SWOOSH_DARK_DMG;
                    playerAnimator.SetTrigger(Consts.DARK_ATTACK);
                    meleeAttackCounter = WeaponsConsts.SWOOSH_DARK_CD;
                    SoundEffect($"DarkBlade{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
                }
                break;
            case (int)EquipmentManager.Items.Axe:
                if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL4_1)] == false)
                {
                    playerAnimator.SetTrigger(Consts.RANGED_ATTACK);
                    Instantiate(axeProjectile, attackPoint.position, attackPoint.rotation).Set(WeaponsConsts.AXE_DMG, WeaponsConsts.AXE_LIFETIME);
                    attackCounter = WeaponsConsts.AXE_CD;
                    SoundEffect($"Axe{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
                }
                else
                {
                    playerAnimator.SetTrigger(Consts.RANGED_ATTACK);
                    Instantiate(goldenAxeProjectile, attackPoint.position, attackPoint.rotation).Set(WeaponsConsts.GOLDEN_AXE_DMG, WeaponsConsts.GOLDEN_AXE_LIFETIME);
                    attackCounter = WeaponsConsts.GOLDEN_AXE_CD;
                    SoundEffect($"GoldenAxe{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
                }
                break;
            case (int)EquipmentManager.Items.Stone:
                playerAnimator.SetTrigger(Consts.RANGED_ATTACK);
                Instantiate(stoneProjectile, attackPoint.position, attackPoint.rotation).Set(WeaponsConsts.STONE_DMG, WeaponsConsts.STONE_LIFETIME);
                attackCounter = WeaponsConsts.STONE_CD;
                SoundEffect($"Stone{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
                break;
            case (int)EquipmentManager.Items.FireSpark:
                playerAnimator.SetTrigger(Consts.RANGED_ATTACK);
                if (!isSwimming)
                {
                    Instantiate(fireSparkProjectile, attackPoint.position, attackPoint.rotation).Set(WeaponsConsts.FIRE_DMG, WeaponsConsts.FIRE_LIFETIME);
                    attackCounter = WeaponsConsts.FIRE_CD;
                    SoundEffect($"Fire{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
                }
                break;
            case (int)EquipmentManager.Items.ArcticBreathe:
                //method ArcticBreathe() in Update()
                break;
            case (int)EquipmentManager.Items.Poison:
                playerAnimator.SetTrigger(Consts.RANGED_ATTACK);
                Instantiate(poisonProjectile, attackPoint.position, attackPoint.rotation).Set(WeaponsConsts.POISON_DMG, WeaponsConsts.POISON_LIFETIME);
                attackCounter = WeaponsConsts.POISON_CD;
                SoundEffect($"Poison{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
                break;
            default:
                break;
        }
    }

    private void MeleeAttack()
    {
        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_3)] == false)
        {
            swooshAttack.GetComponent<DamageEnemy>().damageToDeal = WeaponsConsts.SWOOSH_BASIC_DMG;
            playerAnimator.SetTrigger(Consts.ATTACK);
            meleeAttackCounter = WeaponsConsts.SWOOSH_BASIC_CD;
            SoundEffect($"Swoosh{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
        }
        else
        {
            swooshAttack.GetComponent<DamageEnemy>().damageToDeal = WeaponsConsts.SWOOSH_DARK_DMG;
            playerAnimator.SetTrigger(Consts.DARK_ATTACK);
            meleeAttackCounter = WeaponsConsts.SWOOSH_DARK_CD;
            SoundEffect($"DarkBlade{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
        }
    }

    private void AttackCooldowns()
    {
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }

        if (meleeAttackCounter > 0)
        {
            meleeAttackCounter -= Time.deltaTime;
        }
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

                if (GameManager.isSoundsOn)
                {
                    other.gameObject.GetComponent<AudioSource>().Play();
                }
            }
        }

        if (other.gameObject.name.Equals(Consts.ICEBLOCK))
        {
            isSliding = true;
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
        else if (other.gameObject.name.Equals(Consts.ICEBLOCK))
        {
            if (isSliding)
            {
                velocity = Math.Sign(transform.localScale.x);
                if (transform.localScale.x == 1)
                {
                    playerRB.AddForce(Vector2.right * 75, ForceMode2D.Force);
                }
                else
                {
                    playerRB.AddForce(Vector2.left * 75, ForceMode2D.Force);
                }
            }
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

        if (other.gameObject.name.Equals(Consts.ICEBLOCK))
        {
            velocity = 0;
            isGrounded = true;
            isSliding = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.StartsWith(Consts.WATER_TOP) ||
            other.gameObject.name.StartsWith(Consts.WATER_BOT))
        {
            isSwimming = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.StartsWith(Consts.WATER_TOP) ||
            other.gameObject.name.StartsWith(Consts.WATER_BOT))
        {
            isSwimming = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.StartsWith(Consts.WATER_TOP) ||
            other.gameObject.name.StartsWith(Consts.WATER_BOT))
        {
            isSwimming = false;
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
            if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL4_1)] == false)
            {
                SoundEffect($"Fanfare{UnityEngine.Random.Range(1, 3)}");
            }
            else
            {
                SoundEffect($"Fanfare3");
            }

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
        arcticBreathe.SetActive(false);

        frostBreatheParticles.gameObject.SetActive(false);
        waterBubblesParticles.gameObject.SetActive(false);
        footDustEmission.rateOverTime = 0f;

        deathTimeCounter = deathTime;

        if (playerSounds.Arctic1.isPlaying)
        {
            playerSounds.Arctic1.Stop();
        }
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
            frozenCounter = 0;

            transform.position = GameManager.instance.currentCheckPoint.transform.position;

            var hpController = GetComponentInChildren<PlayerHPController>();
            hpController.currentHP = hpController.maxHP;

            GameManager.instance.PlayerRespawnEffect();
            SoundEffect("Respawn", 1.0f, 1.5f);

            frostBreatheParticles.gameObject.SetActive(SceneManager.GetActiveScene().name.Equals(Consts.LEVEL3_2));
        }
    }

    public void CollectKey()
    {
        keysCount++;
    }

    public void CollectBossItem(string weapon)
    {
        isBossEncounter = false;

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
                case Consts.ARCTIC_BREATHE_WEAPON_COLLECTABLE:
                    objectsAfterBoss.gameObject.transform.position = new Vector3(85.18f, 7.56f, 0f);
                    break;
                case Consts.DARK_WEAPON_COLLECTABLE:
                    objectsAfterBoss.gameObject.transform.position = new Vector3(13.135f, -9.606f, 0f);
                    break;
                case Consts.POISON_WEAPON_COLLECTABLE:
                    objectsAfterBoss.gameObject.transform.position = new Vector3(-17.498f, 2.623f, 0f);
                    break;
                case Consts.GOLDEN_AXE_WEAPON_COLLECTABLE:
                    isGoldenAxeCollected = true;
                    break;
                case Consts.EPIC_TREASURE_COLLECTABLE:
                    isEpicTreasureCollected = true;
                    break;
                default:
                    Debug.Log($"nieznana nazwa przedmiotu z bossa: {weapon}");
                    break;
            }      
        }

        if(isHp5ItemCollected && isStoneWeaponCollected)
        {
            objectsAfterBoss.transform.position = new Vector3(-27.53f, 3.65f, 0f);
        }

        if (isGoldenAxeCollected && isEpicTreasureCollected)
        {
            objectsAfterBoss.transform.position = new Vector3(126.06f, 5.6f, -256.17f);
        }

        GameManager.levelList[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)] = true;
        GameManager.SaveJsonData(GameManager.instance);

        equipmentManager.UpdateEquipment();
    }

    private void UnderwaterBreathe()
    {
        if (isActive && !isLevelCompleted)
        {
            currentOxygen -= Time.deltaTime;
            if (currentOxygen < 0) currentOxygen = 0;
            oxygenBar.fillAmount = currentOxygen / maxOxygen;

            if (currentOxygen < 60)
            {
                oxygenBar.color = new Color32(212, 43, 46, 255);
            }
            else
            {
                oxygenBar.color = new Color(0f + ((maxOxygen - currentOxygen) / 760f), 1.0f - ((maxOxygen - currentOxygen) / 380f), 0.78f, 1f);
            }

            if (currentOxygen == 0)
            {
                Death();
            }
        }
    }

    public void SoundEffect(string name)
    {
        if (GameManager.isSoundsOn)
        {
            playerSounds.GetSound(name).Play();        
        }
    }

    public void SoundEffect(string name, float minPitch, float maxPitch)
    {
        if (GameManager.isSoundsOn)
        {
            var sound = playerSounds.GetSound(name);
            sound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            sound.Play();
        }
    }

    #region Touch control

    public void JumpOnTouch()
    {
        if (isActive && !isLevelCompleted && !GameManager.instance.playerCamera.GetComponent<SmoothFollow>().isLookingUp)
        {
            if (!isSwimming)
            {
                if (currentJumpHangTime > 0f)
                {
                    isGrounded = false;
                    playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                    SoundEffect($"Jump{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
                }
            }
            else
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                SoundEffect($"Jump{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
            }
        }
    }

    public void JumpStopOnTouch()
    {
        if (isActive && !isLevelCompleted && !GameManager.instance.playerCamera.GetComponent<SmoothFollow>().isLookingUp)
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

    public void MeleeAttackButtonDownOnTouch()
    {
        meleeAttackPressed = true;
    }

    public void MeleeAttackButtonUpOnTouch()
    {
        meleeAttackPressed = false;
    }

    public void ChangeItemOnTouch()
    {
        if (isActive)
        {
            attackCounter = 0;
            equipmentManager.ChangeItem();
        }
    }

    public void LeftOnTouch()
    {
        if (isActive && !isLevelCompleted && !isSliding)
        {
            velocity -= 1f;
        }
    }

    public void RightOnTouch()
    {
        if (isActive && !isLevelCompleted && !isSliding)
        {
            velocity += 1f;
        }
    }

    public void MoveStopTouch()
    {
        if (isActive && !isLevelCompleted && !isSliding)
        {
            velocity = 0;
        }
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
        if (isActive && !isLevelCompleted && !isSliding)
        {
            velocity = context.ReadValue<Vector2>().x;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isActive && !isLevelCompleted && !GameManager.instance.playerCamera.GetComponent<SmoothFollow>().isLookingUp)
        {
            if (context.started && currentJumpHangTime > 0f)
            {
                isGrounded = false;
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);

                SoundEffect($"Jump{UnityEngine.Random.Range(1, 4)}", 1.0f, 1.5f);
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
