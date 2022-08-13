using System.Collections.Generic;
using UnityEngine;

public class KingBossBehaviour : MonoBehaviour
{
    public BossActivation activationArea;
    public Animator animator;
    public Transform basePosition;
    public EnemyHPController bossHP;
    public PlayerController player;

    public PolarityDebuff polarityDebuf;
    private PolarityDebuff currentDebuff;
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
        transform.position = basePosition.transform.position;
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        animator.Play("walk", -1, 0f);

        InvokeRepeating("PolarityShift", firstPolarityTime, polarityRepeatingTime);
        InvokeRepeating("SpawnEvilClones", firstSpawnTime, spawnRepeatingTime);
        InvokeRepeating("Teleport", firstTeleportTime, teleportRepeatingTime);
    }

    private void Update()
    {
        if (!player.isBossEncounter || !player.isActive)
        {
            CancelInvoke();

            if(currentDebuff)
            {
                Destroy(currentDebuff.gameObject);
            }

            foreach (var evilClone in evilClonesList)
            {
                if (evilClone != null)
                {
                    Destroy(evilClone.gameObject);
                }
            }

            GetComponentInChildren<EnemyHPController>().ResetHP();
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        evilClonesList.RemoveAll(item => item == null);
    }

    void PolarityShift()
    {
        if (player != null)
        {
            animator.SetTrigger("Shoot");
            Instantiate(debuffParticles, transform.position, transform.rotation);

            plusElectrode.Activate(polarityDamageDelay);
            minusElectrode.Activate(polarityDamageDelay);

            if (currentDebuff == null)
            {
                Instantiate(debuffParticles, player.transform.position, player.transform.rotation);
                currentDebuff = Instantiate(polarityDebuf);
                currentDebuff.isPlus = Random.Range(0, 2) == 0;
                currentDebuff.transform.parent = player.transform;
                currentDebuff.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z);
            }
            else
            {
                var tmp = currentDebuff.isPlus;
                currentDebuff.isPlus = Random.Range(0, 2) == 0;

                if (tmp != currentDebuff.isPlus)
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

        if (isUpper)
        {
            isUpper = false;

            if (currentDebuff.isPlus)
            {
                transform.position = plusElectrode.transform.position;
            }
            else
            {
                transform.position = minusElectrode.transform.position;
            }

            GetComponent<EnemyPatrol>().moveRight = !currentDebuff.isPlus;
        }
        else
        {
            isUpper = true;

            transform.position = basePosition.transform.position;
        }

        Instantiate(teleportEffect, transform.position, transform.rotation);
    }

    private void OnDestroy()
    {
        if (bossHP.currentHP <= 0)
        {
            player.isBossEncounter = false;
           // Instantiate(poisonWeaponLoot, transform.position, transform.rotation);

            activationArea.gameObject.SetActive(false);

            if (currentDebuff)
            {
                Destroy(currentDebuff.gameObject);
            }

            foreach (var evilClone in evilClonesList)
            {
                if (evilClone != null)
                {
                    Destroy(evilClone.gameObject);
                }
            }
        }
    }
}
