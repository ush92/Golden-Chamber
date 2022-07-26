using UnityEngine;

public class EnemyBasicShoot : MonoBehaviour
{
    public GameObject projectile;
    public Transform attackPoint;
    public float firstShootTime;
    public float repeatingTime;

    private void Start()
    {
        InvokeRepeating("Shoot", firstShootTime, repeatingTime);
    }

    private void Shoot()
    {
        Instantiate(projectile, attackPoint.position, attackPoint.rotation);
    }
}
