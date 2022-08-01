using UnityEngine;

public class EnemyBasicShoot : MonoBehaviour
{
    public GameObject projectile;
    public Transform attackPoint;
    public float firstShootTime;
    public float repeatingTime;

    public bool onlyIfFacedToPlayer = false;
    public PlayerController player;

    private void Start()
    {
        InvokeRepeating("Shoot", firstShootTime, repeatingTime);
    }

    private void Shoot()
    {
        if(onlyIfFacedToPlayer)
        {
            if (transform.position.x > player.transform.position.x && !GetComponent<EnemyPatrol>().moveRight ||
                transform.position.x < player.transform.position.x && GetComponent<EnemyPatrol>().moveRight)
            {
                Instantiate(projectile, attackPoint.position, attackPoint.rotation);
            }
        }
        else
        {
            Instantiate(projectile, attackPoint.position, attackPoint.rotation);
        }     
    }
}
