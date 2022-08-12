using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBossBehaviour : MonoBehaviour
{
    public BossActivation activationArea;
    public Animator animator;
    public Transform basePosition;
    public EnemyHPController bossHP;
    public PlayerController player;

    public GameObject polarityDebuf;
    private GameObject currentDebuf;
    public GameObject debuffParticles;

    private void OnEnable()
    {
        player.isBossEncounter = true;

        InvokeRepeating("PolarityShift", 5, 9.0f);
    }

    void Update()
    {
        
    }

    void PolarityShift()
    {
        if (player != null)
        {
            animator.SetTrigger("Shoot");
            Instantiate(debuffParticles, transform.position, transform.rotation);

            currentDebuf = Instantiate(polarityDebuf);
            currentDebuf.transform.parent = player.transform;
            currentDebuf.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.7f, player.transform.position.z);
        }
    }
}
