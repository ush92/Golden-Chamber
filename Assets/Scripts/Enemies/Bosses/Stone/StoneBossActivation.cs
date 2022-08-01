using UnityEngine;

public class StoneBossActivation : MonoBehaviour
{
    public StoneBossBehaviour stoneBoss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            if (stoneBoss != null)
            {
                stoneBoss.gameObject.SetActive(true);
            }
        }
    }
}
