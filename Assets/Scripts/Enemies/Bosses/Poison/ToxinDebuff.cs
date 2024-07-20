using UnityEngine;

public class ToxinDebuff : MonoBehaviour
{
    private GameObject player;
    public PoisonBehaviour boss;
    public GameObject toxinCloud;

    void Start()
    {
        player = GameObject.Find("Player");
        boss = FindFirstObjectByType<PoisonBehaviour>();
        Invoke("Explode", 5);
    }

    void Explode()
    {
        if (boss != null && player != null)
        {
            GameObject toxin = Instantiate(toxinCloud);
            toxin.transform.position = new Vector3(player.transform.position.x, boss.transform.position.y - 0.6f, player.transform.position.z);
            boss.toxinClouds.Add(toxin);
            Destroy(gameObject);
        }
    }
}
