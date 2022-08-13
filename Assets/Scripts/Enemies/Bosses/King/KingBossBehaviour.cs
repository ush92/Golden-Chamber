using System.Collections.Generic;
using UnityEngine;
using static IceBossBehaviour;
using UnityEngine.UIElements.Experimental;

public class KingBossBehaviour : MonoBehaviour
{
    public BossActivation activationArea;
    public Animator animator;
    public Transform basePosition;
    public EnemyHPController bossHP;
    public PlayerController player;

    public PolarityDebuff polarityDebuf;
    private PolarityDebuff currentDebuf;
    public GameObject debuffParticles;
    public float firstPolarityTime;
    public float polarityRepeatingTime;
    public float polarityDamageDelay;

    public ElectricShock plusElectrode;
    public ElectricShock minusElectrode;

    public EnemyPatrol evilClone;
    public List<EnemyPatrol> evilClonesList;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public GameObject spawnEffect;
    public float firstSpawnTime;
    public float spawnRepeatingTime;

    public float firstTeleportTime;
    public float teleportRepeatingTime;
    public GameObject teleportEffect;
    private bool isUpper;

    private void OnEnable()
    {
        player.isBossEncounter = true;
        isUpper = true;
        InvokeRepeating("PolarityShift", firstPolarityTime, polarityRepeatingTime);
        InvokeRepeating("SpawnEvilClones", firstSpawnTime, spawnRepeatingTime);
        InvokeRepeating("Teleport", firstTeleportTime, teleportRepeatingTime);
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

            plusElectrode.Activate(polarityDamageDelay);
            minusElectrode.Activate(polarityDamageDelay);

            if (currentDebuf == null)
            {
                Instantiate(debuffParticles, player.transform.position, player.transform.rotation);
                currentDebuf = Instantiate(polarityDebuf);
                currentDebuf.isPlus = Random.Range(0, 2) == 0;
                currentDebuf.transform.parent = player.transform;
                currentDebuf.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z);
            }
            else
            {
                var tmp = currentDebuf.isPlus;
                currentDebuf.isPlus = Random.Range(0, 2) == 0;
                
                if(tmp != currentDebuf.isPlus)
                {
                    Instantiate(debuffParticles, player.transform.position, player.transform.rotation);
                }
            }
        }
    }

    void SpawnEvilClones()
    {
        evilClonesList.Add(Instantiate(evilClone, new Vector3(leftSpawnPoint.transform.position.x, leftSpawnPoint.transform.position.y), leftSpawnPoint.transform.rotation));
        Instantiate(spawnEffect, new Vector3(leftSpawnPoint.transform.position.x, leftSpawnPoint.transform.position.y), leftSpawnPoint.transform.rotation);

        evilClonesList.Add(Instantiate(evilClone, new Vector3(rightSpawnPoint.transform.position.x, rightSpawnPoint.transform.position.y), rightSpawnPoint.transform.rotation));
        Instantiate(spawnEffect, new Vector3(rightSpawnPoint.transform.position.x, rightSpawnPoint.transform.position.y), rightSpawnPoint.transform.rotation);
    }

    void Teleport()
    {
        Instantiate(teleportEffect, transform.position, transform.rotation);
        animator.SetTrigger("teleport");

        if(isUpper)
        {
            isUpper = false;
            if (currentDebuf.isPlus)
            {
                transform.position = plusElectrode.transform.position;           
            }
            else
            {
                transform.position = minusElectrode.transform.position;
            }

            GetComponent<EnemyPatrol>().moveRight = !currentDebuf.isPlus;
        }
        else
        {
            isUpper = true;
            transform.position = basePosition.transform.position;
        }

        Instantiate(teleportEffect, transform.position, transform.rotation);
    }
}
