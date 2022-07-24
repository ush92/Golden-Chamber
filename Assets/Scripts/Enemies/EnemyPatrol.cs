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

    void Start()
    {
        jumpCooldownCounter = jumpCooldownTime;
    }

    void Update()
    {
        isWallHit = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        isOnGround = Physics2D.OverlapCircle(edgeCheck.position, wallCheckRadius, whatIsWall);

        if (isWallHit || (!isOnGround && !isJumpMode))
        {
            moveRight = !moveRight;
        }

        if (moveRight)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (jumpCooldownCounter > 0)
        {
            jumpCooldownCounter -= Time.deltaTime;
        }
        if (isJumpMode && jumpCooldownCounter <=0)
        {
            jumpCooldownCounter = jumpCooldownTime;
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
        }
    }
}
