using UnityEngine;

public class FireBossBehaviour : MonoBehaviour
{
    public BossActivation activationArea;
    private Vector3 basePosition;

    public EnemyHPController bossHP;
    private bool enraged;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    private bool isWallHit;
    public bool moveRight;

    public Animator animator;
    private float animationRunLength;
    private float currentRunLength;
    public float moveSpeed;
    public float acceleration;

    public int minCountOfRuns;
    public int maxCountOfRuns;
    private int currentCountOfRuns;

    public GameObject projectile;
    public Transform attackPoint;
    public float castingLength;
    private float currentCastingTime;
    public float firstCastTime;
    public float repeatingTime;

    public PlayerController player;
    public GameObject evilSun;
    private float normalEvilSunRepeatingTime;
    private float normalEvilSunDamping;
    public float bossEvilSunRepeatingTime;
    public float bossEvilSunDamping;
    public float enragedEvilSunRepeatingTime;
    public float enragedEvilSunDamping;

    public Collectable fireSparkWeaponLoot;

    private void OnEnable()
    {
        player.isBossEncounter = true;

        basePosition = transform.position;
        currentRunLength = 0;
        moveRight = false;
        enraged = false;

        animationRunLength = animator.GetCurrentAnimatorStateInfo(0).length;
        currentCountOfRuns = Random.Range(minCountOfRuns, maxCountOfRuns);
        animator.SetBool("isCasting", false);

        normalEvilSunRepeatingTime = evilSun.GetComponent<EnemyBasicShoot>().repeatingTime;
        normalEvilSunDamping = evilSun.GetComponent<SmoothFollow>().damping;
        evilSun.GetComponent<EnemyBasicShoot>().ChangeRepeatingTime(bossEvilSunRepeatingTime);
        evilSun.GetComponent<SmoothFollow>().damping = bossEvilSunDamping;
    }

    void Update()
    {
        if (enabled)
        {
            if (currentCountOfRuns > 0)
            {
                animator.SetBool("isCasting", false);
                currentCastingTime = 0;

                isWallHit = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);

                if (isWallHit)
                {
                    moveRight = !moveRight;

                    animator.Play("run", -1, 0f);
                    currentRunLength = 0;

                    currentCountOfRuns--;
                }

                currentRunLength += Time.deltaTime;
                if (currentRunLength >= animationRunLength)
                {
                    currentRunLength = 0;
                }

                if (moveRight)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * currentRunLength * acceleration, GetComponent<Rigidbody2D>().velocity.y);
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed * currentRunLength * acceleration, GetComponent<Rigidbody2D>().velocity.y);
                }
            }
            else
            {
                if (!IsInvoking("CastFireWaves"))
                {
                    InvokeRepeating("CastFireWaves", firstCastTime, repeatingTime);
                }

                currentCastingTime += Time.deltaTime;

                if (currentCastingTime < castingLength)
                {
                    animator.SetBool("isCasting", true);

                    if (transform.position.x > player.transform.position.x)
                    {
                        transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    else if (transform.position.x < player.transform.position.x)
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                }
                else
                {
                    currentCountOfRuns = Random.Range(minCountOfRuns, maxCountOfRuns);
                    CancelInvoke();
                }
            }

            if (bossHP.currentHP <= bossHP.maxHP / 3 && !enraged)
            {
                enraged = true;
                evilSun.GetComponent<EnemyBasicShoot>().ChangeRepeatingTime(enragedEvilSunRepeatingTime);
                evilSun.GetComponent<SmoothFollow>().damping = enragedEvilSunDamping;
            }

            CheckEncounterActivity();
        }
    }

    private void CheckEncounterActivity()
    {       
        if (!player.isBossEncounter || !player.isActive)
        {
            CancelInvoke();
            transform.position = basePosition;
            GetComponentInChildren<EnemyHPController>().ResetHP();

            evilSun.GetComponent<EnemyBasicShoot>().ChangeRepeatingTime(normalEvilSunRepeatingTime);
            evilSun.GetComponent<SmoothFollow>().damping = normalEvilSunDamping;

            enraged = false;
            gameObject.SetActive(false);
        }
    }

    private void CastFireWaves()
    {
        Instantiate(projectile, attackPoint.position, attackPoint.rotation);
    }


    private void OnDestroy()
    {
        if (bossHP.currentHP <= 0)
        {
            evilSun.GetComponent<OnDestroyEffect>().FakeDestroy();
         
            Instantiate(fireSparkWeaponLoot, transform.position, transform.rotation);

            player.isBossEncounter = false;
            activationArea.gameObject.SetActive(false);
        }
    }
}
