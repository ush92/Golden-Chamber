using UnityEngine;

public class FallingDmgObject : MonoBehaviour
{
    public float destroyDelay;
    public GameObject collisionEffect;
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER) || other.tag.Equals(Consts.GROUND))
        {
            GetComponent<AudioSource>().Play();
            animator.SetTrigger("destroy");
            Instantiate(collisionEffect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject, destroyDelay);
        }
    }
}
