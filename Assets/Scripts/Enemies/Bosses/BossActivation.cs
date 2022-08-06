using UnityEngine;

public class BossActivation : MonoBehaviour
{
    public GameObject boss;
    public float delay;
    public static bool isActivated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            if (!isActivated)
            {
                isActivated = true;
                Invoke("ActivateBoss", delay);
            }
        }
    }

    private void ActivateBoss()
    {
        if (boss != null)
        {
            boss.gameObject.SetActive(true);
        }
    }
}
