using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingStone : MonoBehaviour
{
    public float moveSpeed;
    public bool moveRight;
    public bool changeSideOnDamagePlayer = true;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    private bool isWallHit;

    private bool isOnGround;
    public Transform edgeCheck;

    public bool isJumpMode = false;
    public float jumpForce;
    public float jumpCooldownTime;
    private float jumpCooldownCounter;

    public PlayerController player;
    public Animator enemyAnimator;

    public Transform startingPoint;
    public bool isSleepingNow = true;
    private bool justAwaken = true;
    public EnemyHPController enemyHPController;
    private int wakeUpHP;

    void Start()
    {
        jumpCooldownCounter = jumpCooldownTime;

        wakeUpHP = enemyHPController.maxHP;
        enemyHPController.maxHP = 900;
        enemyHPController.currentHP = enemyHPController.maxHP;
    }

    void Update()
    {
        Animate();

        if (isSleepingNow)
        {
            return;
        }
        else
        {
            if (justAwaken)
            {
                justAwaken = false;
                enemyHPController.enabled = true;
                enemyHPController.maxHP = wakeUpHP;
                enemyHPController.currentHP = enemyHPController.maxHP;
            }

            isWallHit = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
            isOnGround = Physics2D.OverlapCircle(edgeCheck.position, wallCheckRadius, whatIsWall);

            if (isWallHit || (!isOnGround && !isJumpMode))
            {
                moveRight = !moveRight;
            }

            if (moveSpeed == 0)
            {
                if (transform.position.x > player.transform.position.x)
                {
                    moveRight = false;
                }
                else if (transform.position.x < player.transform.position.x)
                {
                    moveRight = true;
                }
            }

            if (moveRight)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                GetComponent<Rigidbody2D>().linearVelocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().linearVelocity.y);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().linearVelocity.y);
            }

            if (jumpCooldownCounter > 0)
            {
                jumpCooldownCounter -= Time.deltaTime;
            }
            if (isJumpMode && jumpCooldownCounter <= 0)
            {
                jumpCooldownCounter = jumpCooldownTime;
                GetComponent<Rigidbody2D>().linearVelocity = new Vector2(GetComponent<Rigidbody2D>().linearVelocity.x, jumpForce);
            }
        }
    }
    private void Animate()
    {
        if (isSleepingNow)
        {
            enemyAnimator.SetBool("sleeping", true);
        }
        else
        {
            enemyAnimator.SetBool("sleeping", false);
        }
    }
}
