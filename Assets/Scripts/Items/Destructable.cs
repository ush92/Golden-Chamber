using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject destructEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER) || other.tag.Equals(Consts.PLAYER_PROJECTILE))
        {
            Instantiate(destructEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
