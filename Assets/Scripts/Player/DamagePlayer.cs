using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damageToDeal = 1;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            var playerHPController = other.GetComponent<PlayerHPController>();
            var player = other.GetComponent<PlayerController>();

            playerHPController.DamagePlayer(damageToDeal);
            
            if (player.isAlive)
            {
                player.knockbackCounter = player.knockbackLength;
                player.knockbackFromRight = other.transform.position.x < transform.position.x ? true : false;
            }

            if (tag.Equals(Consts.ENEMY))
            {
                var patrol = this.GetComponent<EnemyPatrol>();
                patrol.moveRight = !patrol.moveRight;
            }
        }
    }
}
