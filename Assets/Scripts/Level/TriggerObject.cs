using UnityEngine;
using UnityEngine.UI;

public class TriggerObject : MonoBehaviour
{
    public string triggerName;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            other.GetComponent<PlayerController>().triggerObject = triggerName;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            other.GetComponent<PlayerController>().triggerObject = "";
        }
    }
}
