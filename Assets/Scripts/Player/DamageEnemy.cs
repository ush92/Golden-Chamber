using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public int damageToDeal;
    private bool isDamageDone = false;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.ENEMY) && !isDamageDone)
        {
            other.GetComponent<EnemyHPController>().takeDamage(damageToDeal);
            isDamageDone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDamageDone)
        {
            isDamageDone = false;
        }
    }
}
