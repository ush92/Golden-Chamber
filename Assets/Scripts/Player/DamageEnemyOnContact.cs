using UnityEngine;

public class DamageEnemyOnContact : MonoBehaviour
{
    public int damageToDeal;

    public float bounceOnEnemy;
    private Rigidbody2D playerRB;

    void Start()
    {
       playerRB = transform.parent.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.ENEMY))
        {
            other.GetComponent<EnemyHPController>().takeDamage(damageToDeal);

            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, bounceOnEnemy);
        }
    }
}
