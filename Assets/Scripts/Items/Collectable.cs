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
            
            Instantiate(pickupEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
