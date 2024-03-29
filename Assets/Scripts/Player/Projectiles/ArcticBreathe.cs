using UnityEngine;

public class ArcticBreathe : MonoBehaviour
{
    private int damagePerTick;
    private bool isDamageDone;
    private float damageCooldown;
    private float cooldownTimer;
    public PlayerController playerController;
    public ParticleSystem particles;

    public Animator animator;

    void Start()
    {
        damagePerTick = WeaponsConsts.ICE_TICK_DMG;
        cooldownTimer = WeaponsConsts.ICE_CD;
    }

    void OnEnable()
    {
        isDamageDone = false;
    }

    void Update()
    {
        if (enabled)
        {
            if (isDamageDone)
            {
                cooldownTimer -= Time.deltaTime;
            }

            if (cooldownTimer <= 0)
            {
                isDamageDone = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.ENEMY) && !isDamageDone)
        {
            other.GetComponent<EnemyHPController>().takeDamage(damagePerTick);
            isDamageDone = true;
            cooldownTimer = damageCooldown;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.ENEMY) && !isDamageDone)
        {
            other.GetComponent<EnemyHPController>().takeDamage(damagePerTick);
        }

        if (other.tag.Equals(Consts.GROUND))
        {
            animator.Play("arcticBreathe", -1, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //
    }
}
