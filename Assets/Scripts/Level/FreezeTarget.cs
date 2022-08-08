using UnityEngine;

public class FreezeTarget : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            other.GetComponent<PlayerController>().frozenCounter = 9999f;
            other.GetComponent<PlayerController>().isFrozen = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            other.GetComponent<PlayerController>().frozenCounter = 9999f;
            other.GetComponent<PlayerController>().isFrozen = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            other.GetComponent<PlayerController>().frozenCounter = 0;
            other.GetComponent<PlayerController>().isFrozen = false;
        }
    }
}
