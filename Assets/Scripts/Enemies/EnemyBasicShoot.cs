using UnityEngine;

public class EnemyBasicShoot : MonoBehaviour
{
    public GameObject projectile;
    public Transform attackPoint;
    public float firstShootTime;
    public float repeatingTime;
    public float minDistanceFromPlayer = 2.0f;
    public bool onlyIfFacedToPlayer = false;
    public PlayerController player;

    public Animator animator;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        InvokeRepeating("Shoot", firstShootTime, repeatingTime);
    }

    public void ChangeRepeatingTime(float newTime)
    {
        CancelInvoke();
        InvokeRepeating("Shoot", 0, newTime);
    }

    public void ChangeRepeatingTime(float delay, float newTime)
    {
        CancelInvoke();
        InvokeRepeating("Shoot", delay, newTime);
    }

    private void Shoot()
    {     
        if(onlyIfFacedToPlayer && player) //+/- 2.0f to block shooting if player is too close
        {
            if (transform.position.x - minDistanceFromPlayer > player.transform.position.x && !GetComponent<EnemyPatrol>().moveRight ||
                transform.position.x + minDistanceFromPlayer < player.transform.position.x && GetComponent<EnemyPatrol>().moveRight)
            {
                Instantiate(projectile, attackPoint.position, attackPoint.rotation);

                if (animator != null)
                {
                    animator.SetTrigger("Shoot");
                }
            }
        }
        else
        {
            Instantiate(projectile, attackPoint.position, attackPoint.rotation);

            if (animator != null)
            {
                animator.SetTrigger("Shoot");
            }
        }     
    }
}
