using UnityEngine;

public class EnemyPatrol : MonoBehaviour
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

    void Start()
    {
        jumpCooldownCounter = jumpCooldownTime;
    }

    void Update()
    {
        isWallHit = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        if (edgeCheck != null)
        {
            isOnGround = Physics2D.OverlapCircle(edgeCheck.position, wallCheckRadius, whatIsWall);
        }
        else
        {
            isOnGround = true;
        }

        if (isWallHit || (!isOnGround && !isJumpMode))
        {
            moveRight = !moveRight;
        }

        if(moveSpeed == 0) 
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
        if (isJumpMode && jumpCooldownCounter <=0)
        {
            jumpCooldownCounter = jumpCooldownTime;
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(GetComponent<Rigidbody2D>().linearVelocity.x, jumpForce);
        }
    }
}
