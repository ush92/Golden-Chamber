using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed;
    public bool moveRight;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    private bool isWallHit;

    private bool isOnGround;
    public Transform edgeCheck;

    void Start()
    {
        
    }

    void Update()
    {
        isWallHit = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        isOnGround = Physics2D.OverlapCircle(edgeCheck.position, wallCheckRadius, whatIsWall);

        if (isWallHit || !isOnGround)
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
    }
}
