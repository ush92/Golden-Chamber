using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            if(gameObject.name.Equals(Consts.HP_ITEM_SMALL))
            {
                var playerHPController = other.GetComponent<PlayerHPController>();

                if (playerHPController.currentHP == playerHPController.maxHP)
                {
                    return;
                }

                playerHPController.HealPlayer(1);
            }
            else if (gameObject.name.Equals(Consts.KEY))
            {
                var playerController = other.GetComponent<PlayerController>();
                playerController.CollectKey();
            }
            else
            {
                FindObjectOfType<CollectablesController>().Collect(gameObject.name);
            }

            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 2.0f);
        }
    }
}
