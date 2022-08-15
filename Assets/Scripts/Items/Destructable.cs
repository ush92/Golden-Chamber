using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject destructEffect;
    public AudioSource Box1, Box2, Urn1, Urn2;
    private bool isDestructed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            var playerRB = other.GetComponent<Rigidbody2D>();
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * -0.33f);

            Instantiate(destructEffect, transform.position, transform.rotation);
            PlaySound();

            DeactivateAndDestroy();
        }
        else if (other.tag.Equals(Consts.PLAYER_PROJECTILE))
        {
            if (other.name.Equals(Consts.PLAYER_ATTACK_WAVE))
            {
                if (!isDestructed)
                {
                    isDestructed = true;
                    other.gameObject.SetActive(false);

                    Instantiate(destructEffect, transform.position, transform.rotation);
                    PlaySound();

                    DeactivateAndDestroy();
                }
            }
            else if (other.name.Contains(Consts.PLAYER_STONE))
            {
                Destroy(other.gameObject);

                Instantiate(destructEffect, transform.position, transform.rotation);
                PlaySound();

                DeactivateAndDestroy();
            }
            else if (other.name.Equals(Consts.PLAYER_ARCTIC_BREATHE))
            {
                return;
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void DeactivateAndDestroy()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        foreach (var collider in gameObject.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        Destroy(gameObject, 3.0f);
    }

    private void PlaySound()
    {
        if (GameManager.isSoundsOn)
        {
            if (name.Contains("Box"))
            {
                if (Random.Range(0, 2) == 0)
                {
                    Box1.Play();
                }
                else
                {
                    Box2.Play();
                }
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    Urn1.Play();
                }
                else
                {
                    Urn2.Play();
                }
            }
        }
    }
}
