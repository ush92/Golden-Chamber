using UnityEngine;
using static UnityEngine.ParticleSystem;

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
        //if (playerController.transform.localScale.x == 0)
        //{
        //    particles.transform.rotation = Quaternion.Euler(0, 0, 0);
        //}
        //else
        //{
        //    particles.transform.rotation = Quaternion.Euler(0, -180, 0);
        //}

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

        if (playerController.velocity != 0 || !playerController.isGrounded)
        {
            gameObject.SetActive(false);
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
