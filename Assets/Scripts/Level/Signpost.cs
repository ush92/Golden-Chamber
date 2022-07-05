using UnityEngine;

public class Signpost : MonoBehaviour
{
    public GameObject textarea;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            textarea.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            textarea.gameObject.SetActive(false);
        }
    }
}
