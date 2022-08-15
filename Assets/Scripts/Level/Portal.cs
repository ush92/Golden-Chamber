using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform exitPoint;
    public GameObject teleportEffect;
    public AudioSource sound1;
    public AudioSource sound2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            Instantiate(teleportEffect, transform.position, transform.rotation);       
            other.transform.position = exitPoint.position;        
            Instantiate(teleportEffect, exitPoint.position, exitPoint.rotation);

            if(Random.Range(0,2) == 0)
            {
                sound1.Play();
            }
            else
            {
                sound2.Play();
            }
        }
    }
}
