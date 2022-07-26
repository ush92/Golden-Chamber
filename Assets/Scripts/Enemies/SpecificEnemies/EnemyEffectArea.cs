using UnityEngine;

public class EnemyEffectArea : MonoBehaviour
{
    public FlyingAround flyingEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            flyingEnemy.shouldAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            if (flyingEnemy.withReturn)
            {
                flyingEnemy.shouldAttack = false;
            }
        }
    }
}
