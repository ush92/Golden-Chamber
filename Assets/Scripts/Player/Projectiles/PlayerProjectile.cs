using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float heigthPower;
    private PlayerController player;
    private float playerVelocityX;

    private float lifetime;
    private int damageToDeal;
    private bool isDamageDone = false;

    public bool isCollidingWithEnv = true;
    public GameObject collisionEffect;
    private bool isCollided;

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

    public void Set(int damage, float time)
    {
        damageToDeal = damage;
        lifetime = time;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isCollided = true;

        if (name.Contains(Consts.PLAYER_POISON) && other.tag.Equals(Consts.ENEMY))
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Animator>().SetTrigger("triggered");
            lifetime = WeaponsConsts.POISON_TRIGGERED_LIFETIME;
            //rest in trigger STAY
        }
        else if (other.tag.Equals(Consts.ENEMY) && !isDamageDone)
        {
            other.GetComponent<EnemyHPController>().takeDamage(damageToDeal);

            if (!name.Contains(Consts.PLAYER_STONE))
            {
                isDamageDone = true;
                DestroyProjectile();
            }
        }
        else if(other.tag.Equals(Consts.GROUND) && isCollidingWithEnv)
        {
            if (!name.Contains(Consts.PLAYER_FIRESPARK))
            {
                rb.velocity = new Vector2(0, 0);
                DestroyProjectile();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (name.Contains(Consts.PLAYER_POISON) && other.tag.Equals(Consts.ENEMY))
        {
            other.GetComponent<EnemyHPController>().takeDamage(damageToDeal);
            Instantiate(collisionEffect, transform.position, Quaternion.Euler(0, 0, 0));
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

            Instantiate(collisionEffect, transform.position, Quaternion.Euler(0, 0, 0));
            
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            foreach(var collider in gameObject.GetComponents<Collider2D>())
            {
                collider.enabled = false;
            }
            
            Destroy(gameObject, 2.0f);
        }
    }
}
