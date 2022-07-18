using UnityEngine;

public class PociupalaBehaviour : MonoBehaviour
{
    private float moveSpeed = 1;
    public float moveSpeedAcceleration;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;

    public Transform edgeCheck;

    public float attackDistance;
    private bool isAttacking = false;
    public float attackingTimeLength;
    private float attackingTimeCounter;

    public Animator animator;

    public PlayerController player;

    public GameObject[] saws;
    private int sawIndex = -1;
    private bool isSawJump = false;
    private Vector3 sawBasePoistion;
    public float sawMoveSpeed;

    void Start()
    {
        InvokeRepeating("SawJumping", 5.0f, 7.0f);
        InvokeRepeating("SawReturn", 10.0f, 5.0f);

        Invoke("SetNormalMoveSpeed", 3.0f);
    }

    private void SetNormalMoveSpeed()
    {
        moveSpeed = 8.0f;
    }

    void Update()
    {
        if (isAttacking)
        {
            attackingTimeCounter -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(player.gameObject.transform.position.x, transform.position.y), moveSpeed * Time.deltaTime);
        }

        moveSpeed += Time.deltaTime * moveSpeedAcceleration;

        if (sawIndex != -1)
        {
            if (isSawJump)
            {
                saws[sawIndex].gameObject.transform.position = Vector2.MoveTowards(saws[sawIndex].gameObject.transform.position,
                    player.transform.position, sawMoveSpeed * Time.deltaTime);
            }
            else
            {
                saws[sawIndex].gameObject.transform.position = Vector2.MoveTowards(saws[sawIndex].gameObject.transform.position,
                    sawBasePoistion, sawMoveSpeed * Time.deltaTime * 6);
            }
        }

        if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (transform.position.x < player.transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            moveSpeed = 0;
        }

        if (isAttacking == false && Vector2.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            isAttacking = true;
            attackingTimeCounter = attackingTimeLength;

            moveSpeed = 0;
        }

        if (attackingTimeCounter <= 0)
        {
            isAttacking = false;
        }

        Animate();
    }

    private void Animate()
    {
        if (player.isActive)
        {
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isPlayerAlive", true);
        }
        else
        {
            moveSpeed = 0;
            animator.SetBool("isAttacking", false);
            animator.SetBool("isPlayerAlive", false);
        }     
    }

    private void SawJumping()
    {
        sawIndex = Random.Range(0, 11);
        sawBasePoistion = saws[sawIndex].gameObject.transform.position;
        isSawJump = true;
    }

    private void SawReturn()
    {
        isSawJump = false; 
    }

    private void OnDestroy()
    {
        saws[sawIndex].gameObject.transform.position = sawBasePoistion;
    }
}
