using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject destructEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            Instantiate(destructEffect, transform.position, transform.rotation);
            Destroy(gameObject, 0.05f);
        }
        else if (other.tag.Equals(Consts.PLAYER_PROJECTILE))
        {
            if (other.name.Equals(Consts.PLAYER_ATTACK_WAVE))
            {
                Instantiate(destructEffect, transform.position, transform.rotation);
                Destroy(gameObject, 0.05f);
                other.gameObject.SetActive(false);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER_PROJECTILE))
        {
           //
        }
    }
}
