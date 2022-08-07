using UnityEngine;

public class ArcticBreathe : MonoBehaviour
{
    public int damagePerTick;
    private bool isDamageDone;
    public float damageCooldown;
    private float cooldownTimer;
    public PlayerController playerController;
    public ParticleSystem particles;

    void Start()
    {
        isDamageDone = false;
        cooldownTimer = damageCooldown;
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
    }
}
