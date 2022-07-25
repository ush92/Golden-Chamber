using UnityEngine;

public class PlayerAxeProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float heigthPower;
    private PlayerController player;
    private float playerVelocityX;

    public int damageToDeal;
    private bool isDamageDone = false;

    public GameObject collisionEffect;
    private bool isCollided;

    private float lifetime = 3.0f;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerVelocityX = player.playerRB.velocity.x;

        if (player.transform.localScale.x < 0)
        {
            speed = -speed;
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1f);
        }

        rb.velocity = new Vector2(speed, heigthPower);
    }

    void Update()
    {
        if (!isCollided)
        {
            rb.velocity = new Vector2(speed + playerVelocityX / 2.0f, rb.velocity.y);
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isCollided = true;

        if (other.tag.Equals(Consts.ENEMY) && !isDamageDone)
        {
            other.GetComponent<EnemyHPController>().takeDamage(damageToDeal);
            isDamageDone = true;

            Destroy(gameObject);
        }
        else if(other.tag.Equals(Consts.GROUND))
        {       
            rb.velocity = new Vector2(0, 0);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isCollided = true;

        if (isDamageDone)
        {
            isDamageDone = false;
        }
    }

    private void OnDestroy()
    {
        Instantiate(collisionEffect, transform.position, Quaternion.Euler(0, 0, 0));
    }
}
