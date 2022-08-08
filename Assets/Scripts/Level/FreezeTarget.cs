using UnityEngine;

public class FreezeTarget : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            other.GetComponent<PlayerController>().frozenLength = 999999f;
            other.GetComponent<PlayerController>().isFrozen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            other.GetComponent<PlayerController>().frozenCounter = 0;
        }
    }
}
