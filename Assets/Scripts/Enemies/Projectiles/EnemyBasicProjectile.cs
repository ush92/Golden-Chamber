using UnityEngine;

public class EnemyBasicProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float heigthPower;

    public GameObject collisionEffect;

    private float lifetime = 3.0f;

    public int damageToDeal = 1;
    private bool damageDone = false;

    private PlayerController player;
    public bool aimPlayer = false;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if (aimPlayer)
        {
            rb.velocity = (player.transform.position - transform.position).normalized * speed;
        }
        else
        {
            rb.velocity = new Vector2(speed, heigthPower);
        }
    }


    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Equals(Consts.PLAYER))
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER) && damageToDeal > 0 && !damageDone)
        {
            damageDone = true;

            var playerHPController = other.GetComponent<PlayerHPController>();
            var player = other.GetComponent<PlayerController>();

            playerHPController.DamagePlayer(damageToDeal);

            if (player.isActive && player.knockbackCooldownCounter <= 0)
            {
                player.knockbackCounter = player.knockbackLength;
                player.knockbackFromRight = other.transform.position.x < transform.position.x ? true : false;

                player.knockbackCooldownCounter = player.knockbackCooldownLength;
            }
        }

        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        //AudioSource audio = GetComponent<AudioSource>();
        //audio.Play();

        Instantiate(collisionEffect, transform.position, Quaternion.Euler(0, 0, 0));

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 2.0f);
    }
}