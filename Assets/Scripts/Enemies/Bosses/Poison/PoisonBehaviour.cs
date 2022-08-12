using UnityEngine;

public class PoisonBehaviour : MonoBehaviour
{
    public PlayerController player;
    public GameObject toxinDebuf;

    void Start()
    {
        InvokeRepeating("InfectPlayer", 5, 8.5f);
    }

    void InfectPlayer()
    {
        if (player != null)
        {
            GameObject toxin = Instantiate(toxinDebuf);
            toxin.transform.parent = player.transform;
            toxin.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.6f, player.transform.position.z);
        }
    }
}
