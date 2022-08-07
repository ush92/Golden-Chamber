using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float heigthPower;
    private PlayerController player;
    private float playerVelocityX;

    public int damageToDeal;
    private bool isDamageDone = false;

    public bool isCollidingWithEnv = true;
    public GameObject collisionEffect;
    private bool isCollided;

    public float lifetime = 3.0f;

    private bool isDestroyed = false;

    public bool usePhysics = false;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerVelocityX = player.playerRB.velocity.x;

        if (player.transform.localScale.x < 0)
        {
            speed = -speed;

            if (!name.Contains(Consts.PLAYER_POISON)) //poison keeps own scale
            {
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1f);
            }
        }

        rb.velocity = new Vector2(speed, heigthPower);
    }

    void Update()
    {
        if (!isCollided && !usePhysics)
        {
            rb.velocity = new Vector2(speed + playerVelocityX / 2.0f, rb.velocity.y);
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isCollided = true;

        if (name.Contains(Consts.PLAYER_POISON) && other.tag.Equals(Consts.ENEMY) && !isDamageDone)
        {
            GetComponent<CapsuleCollider2D>().enabled = true;

            GetComponent<Animator>().SetTrigger("triggered");
            other.GetComponent<EnemyHPController>().takeDamage(damageToDeal);
            Instantiate(collisionEffect, transform.position, Quaternion.Euler(0, 0, 0));
            lifetime = 0.50f;
        }
        else if (other.tag.Equals(Consts.ENEMY) && !isDamageDone)
        {
            other.GetComponent<EnemyHPController>().takeDamage(damageToDeal);
            isDamageDone = true;

            DestroyProjectile();
        }
        else if(other.tag.Equals(Consts.GROUND) && isCollidingWithEnv)
        {       
            rb.velocity = new Vector2(0, 0);
            DestroyProjectile();
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

    private void DestroyProjectile()
    {
        if (isDestroyed == false)
        {
            isDestroyed = true;

            //AudioSource audio = GetComponent<AudioSource>();
            //audio.Play();

            if (!name.Contains(Consts.PLAYER_POISON))
            {
                Instantiate(collisionEffect, transform.position, Quaternion.Euler(0, 0, 0));
            }

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 2.0f);
        }
    }
}
