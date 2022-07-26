using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damageToDeal = 1;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER) && damageToDeal > 0)
        {
            var playerHPController = other.GetComponent<PlayerHPController>();
            var player = other.GetComponent<PlayerController>();

            playerHPController.DamagePlayer(damageToDeal);

            if (player.isActive && player.knockbackCooldownCounter <= 0)
            {
                player.knockbackCounter = player.knockbackLength;
                player.knockbackFromRight = other.transform.position.x < transform.position.x ? true : false;

                player.knockbackCooldownCounter = player.knockbackCooldownLength;
            }
           
            if (tag.Equals(Consts.ENEMY))
            {
                if (TryGetComponent<EnemyPatrol>(out var patrol))
                {
                    if (patrol.changeSideOnDamagePlayer)
                    {
                        patrol.moveRight = !patrol.moveRight;
                    }
                }
            }
        }
    }
}
