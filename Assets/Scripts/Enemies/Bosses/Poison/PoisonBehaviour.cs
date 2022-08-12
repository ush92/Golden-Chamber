using System.Collections.Generic;
using UnityEngine;

public class PoisonBehaviour : MonoBehaviour
{
    public PlayerController player;
    public GameObject toxinDebuf;
    private GameObject currentDebuf;

    public Animator animator;
    public GameObject debuffParticles;

    public List<GameObject> toxinClouds;

    void Start()
    {
        InvokeRepeating("InfectPlayer", 5, 8.5f);
    }

    void InfectPlayer()
    {
        if (player != null)
        {
            animator.SetTrigger("Shoot");
            Instantiate(debuffParticles, transform.position, transform.rotation);
            currentDebuf = Instantiate(toxinDebuf);
            currentDebuf.transform.parent = player.transform;
            currentDebuf.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.6f, player.transform.position.z);
        }
    }

    private void OnDestroy()
    {
        Destroy(currentDebuf);
        foreach(var cloud in toxinClouds)
        {
            Destroy(cloud);
        }
    }
}
