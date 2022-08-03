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
                playerController.CollectBossItem(Consts.AXE_WEAPON_COLLECTABLE);
            }
            else if (gameObject.name.Contains(Consts.STONE_WEAPON_COLLECTABLE))
            {
                playerController.CollectBossItem(Consts.STONE_WEAPON_COLLECTABLE);
            }
            else if (gameObject.name.Contains(Consts.HP_MAX_PLUS_5))
            {
                playerHPController.maxHP = 10;
                playerHPController.HealPlayer(10);
                playerController.CollectBossItem(Consts.HP_MAX_PLUS_5);
            }
            else
            {
                FindObjectOfType<CollectablesController>().Collect(gameObject.name);
            }

            if (GameManager.isSoundsOn)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
            }

            Instantiate(pickupEffect, transform.position, Quaternion.Euler(0,0,0));

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 2.0f);
        }
    }
}
