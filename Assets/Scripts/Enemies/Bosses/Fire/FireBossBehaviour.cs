using UnityEngine;

public class FireBossBehaviour : MonoBehaviour
{
    public BossActivation activationArea;
    public Vector3 basePosition;

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

    public MusicManager musicManager;

    private void OnEnable()
    {
        player.isBossEncounter = true;

        basePosition = transform.position;

        animationRunLength = animator.GetCurrentAnimatorStateInfo(0).length;
        currentCountOfRuns = Random.Range(minCountOfRuns, maxCountOfRuns);
        animator.SetBool("isCasting", false);

        normalEvilSunRepeatingTime = evilSun.GetComponent<EnemyBasicShoot>().repeatingTime;
        normalEvilSunDamping = evilSun.GetComponent<SmoothFollow>().damping;
        evilSun.GetComponent<EnemyBasicShoot>().repeatingTime = bossEvilSunRepeatingTime;
        evilSun.GetComponent<SmoothFollow>().damping = bossEvilSunDamping;

        musicManager.SwitchToSecondaryTheme();
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
                if (!IsInvoking("CastFireWave"))
                {
                    InvokeRepeating("CastFireWave", firstCastTime, repeatingTime);
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

            CheckEncounterActivity();
        }
    }

    private void CheckEncounterActivity()
    {
        if (!player.isBossEncounter || !player.isActive)
        {
            BossActivation.isActivated = false;

            CancelInvoke();
            transform.position = basePosition;
            GetComponentInChildren<EnemyHPController>().ResetHP();
           
            evilSun.GetComponent<EnemyBasicShoot>().repeatingTime = normalEvilSunRepeatingTime;
            evilSun.GetComponent<SmoothFollow>().damping = normalEvilSunDamping;

            musicManager.SwitchToPrimaryTheme();

            gameObject.SetActive(false);
        }
    }

    private void CastFireWave()
    {
        Instantiate(projectile, attackPoint.position, attackPoint.rotation);
    }

    private void OnDestroy()
    {
        activationArea.gameObject.SetActive(false);

        evilSun.GetComponent<OnDestroyEffect>().FakeDestroy();
        evilSun.gameObject.GetComponent<EnemyBasicShoot>().CancelInvoke();
        evilSun.gameObject.SetActive(false);
    }
}
