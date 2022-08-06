using UnityEngine;

public class BossActivation : MonoBehaviour
{
    public GameObject boss;
    public float delay;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            if (boss != null && !boss.gameObject.activeSelf)
            {
                Invoke("ActivateBoss", delay);
            }
        }
    }

    private void ActivateBoss()
    {
        boss.gameObject.SetActive(true);       
    }
}
