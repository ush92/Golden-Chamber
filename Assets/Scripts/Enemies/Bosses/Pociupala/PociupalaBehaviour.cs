using UnityEngine;

public class PociupalaBehaviour : MonoBehaviour
{
    private Vector3 basePosition;

    private float moveSpeed = 0.5f;
    public float moveSpeedAcceleration;

    public float attackDistance;
    private bool isAttacking = false;
    public float attackingTimeLength;
    private float attackingTimeCounter;

    public Animator animator;

    public PlayerController player;

    public GameObject[] saws;
    private int sawIndex = -1;
    private bool isSawJump = false;
    private Vector3 sawBasePosition;
    public float sawMoveSpeed;

    public Collectable axeWeaponLoot;

    void Start()
    {

    }

    private void OnEnable()
    {
        player.isBossEncounter = true;
        basePosition = transform.position;      

        animator.SetBool("isAttacking", false);
        animator.SetBool("isPlayerAlive", true);

        InvokeRepeating("SawJumping", 5.0f, 7.0f);
        InvokeRepeating("SawReturn", 10.0f, 5.0f);

        Invoke("SetNormalMoveSpeed", 3.0f);

        GetComponent<SpriteRenderer>().color = Color.white;
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
                    sawBasePosition, sawMoveSpeed * Time.deltaTime * 6);
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

        moveSpeed += Time.deltaTime * moveSpeedAcceleration;

        Animate();

        CheckEncounterActivity();
    }

    private void CheckEncounterActivity()
    {
        if (enabled && !player.isBossEncounter)
        {
            CancelInvoke();
            
            isSawJump = false;
            saws[sawIndex].gameObject.transform.position = sawBasePosition;
            
            transform.position = basePosition;
            GetComponentInChildren<EnemyHPController>().ResetHP();
            GetComponent<SpriteRenderer>().color = Color.white;      
            gameObject.SetActive(false);
        }
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
        sawBasePosition = saws[sawIndex].gameObject.transform.position;
        isSawJump = true;
    }

    private void SawReturn()
    {
        isSawJump = false; 
    }

    private void OnDestroy()
    {
        saws[sawIndex].gameObject.transform.position = sawBasePosition;

        Instantiate(axeWeaponLoot, transform.position, transform.rotation);
    }
}
