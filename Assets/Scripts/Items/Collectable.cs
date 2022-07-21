using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            var playerHPController = other.GetComponent<PlayerHPController>();
            var playerController = other.GetComponent<PlayerController>();

            if (gameObject.name.Equals(Consts.HP_ITEM_SMALL))
            {
                if (playerHPController.currentHP == playerHPController.maxHP)
                {
                    return;
                }
                playerHPController.HealPlayer(1);
            }
            else if (gameObject.name.Equals(Consts.KEY))
            {
                playerController.CollectKey();
            }
            else if (gameObject.name.Contains(Consts.AXE_WEAPON_COLLECTABLE))
            {
                playerController.CollectWeapon(Consts.AXE_WEAPON_COLLECTABLE);
            }
            else
            {
                FindObjectOfType<CollectablesController>().Collect(gameObject.name);
            }

            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            Instantiate(pickupEffect, transform.position, Quaternion.Euler(0,0,0));

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 2.0f);
        }
    }
}
