using System.Collections.Generic;
using UnityEngine;

public class DarkBossBehaviour : MonoBehaviour
{
    public BossActivation activationArea;
    public EnemyHPController bossHP;

    public float startDelay;
    public Animator animator;
    public Transform basePosition;
    public Transform bottomPosition;
    public List<Transform> upperPositions;
    private bool isUpper;

    public float teleportRepeatingTime;
    public GameObject teleportEffect;

    public EnemyBasicShoot shooting;
    public float normalCastingCooldown;
    public float enragedCastingCooldown;

    public Transform spiderSpawnPoint;
    public float spawnRepeatingTime;
    public float enragedSpawnRepeatingTime;
    public List<EnemyPatrol> spidersList;
    public EnemyPatrol spider;
    public float spidersNormalMoveSpeed;
    public float spidersEnragedMoveSpeed;
    public List<SpriteRenderer> spidersCounterIndicators;

    public GameObject portals;
    public GameObject portalsRemoveEffect;

    public GameObject enrageTimer;
    public float enrageTime;
    public int spidersToEnrageCount;
    private bool isEnraged;

    public PlayerController player;
    public Collectable darkWeaponLoot;

    private void OnEnable()
    {
        player.isBossEncounter = true;

        transform.position = basePosition.transform.position;
        isUpper = true;
        isEnraged = false;
        enrageTimer.SetActive(true);
        spider.moveSpeed = spidersNormalMoveSpeed;
        spidersList.RemoveAll(item => item == null);
        animator.Play("idle", -1, 0f);

        shooting.ChangeRepeatingTime(3.0f, normalCastingCooldown);
        InvokeRepeating("Teleport", startDelay, teleportRepeatingTime);
        InvokeRepeating("SpawnSpiders", startDelay, spawnRepeatingTime);
        Invoke("RemovePortals", enrageTime);
    }

    private void FixedUpdate()
    {
        spidersList.RemoveAll(item => item == null);

        if (!isEnraged)
        {
            for (int i = 0; i < spidersCounterIndicators.Count; i++)
            {
                if (spidersList.Count > i)
                {
                    spidersCounterIndicators[i].color = new Color32(255, 0, 255, 200);
                }
                else
                {
                    spidersCounterIndicators[i].color = new Color32(0, 0, 0, 155);
                }
            }
        }
        else
        {
            for (int i = 0; i < spidersCounterIndicators.Count; i++)
            {
                spidersCounterIndicators[i].color = new Color32(255, 0, 0, 200);       
            }
        }
    }

    void Update()
    {
        if(spidersList.Count >= spidersToEnrageCount && !isEnraged)
        {
            isEnraged = true;
            Invoke("RemovePortals", 0);
        }

        if (bossHP.currentHP <= 0 && player.isBossEncounter)
        {
            CancelInvoke();
            player.isBossEncounter = false;
            Instantiate(darkWeaponLoot, transform.position, transform.rotation);
            activationArea.gameObject.SetActive(false);
            portals.SetActive(false);
            enrageTimer.SetActive(false);

            foreach (var spawnedSpider in spidersList)
            {
                if (spawnedSpider != null)
                {
                    Destroy(spawnedSpider.gameObject);
                }
            }
            for (int i = 0; i < spidersCounterIndicators.Count; i++)
            {
                spidersCounterIndicators[i].color = new Color32(0, 0, 0, 155);
            }
        }
        else if (bossHP.currentHP > 0 && (!player.isBossEncounter || !player.isActive))
        {
            CancelInvoke();
            shooting.CancelInvoke();

            foreach (var spawnedSpider in spidersList)
            {
                if (spawnedSpider != null)
                {
                    Destroy(spawnedSpider.gameObject);
                }
            }

            for (int i = 0; i < spidersCounterIndicators.Count; i++)
            {
                spidersCounterIndicators[i].color = new Color32(0, 0, 0, 155);
            }

            isEnraged = false;
            enrageTimer.SetActive(false);
            portals.SetActive(true);
            GetComponentInChildren<EnemyHPController>().ResetHP();
            gameObject.SetActive(false);
        }
    }

    void Teleport()
    {
        animator.SetTrigger("teleport");
        Instantiate(teleportEffect, transform.position, transform.rotation);

        if (isUpper)
        {
            transform.position = bottomPosition.transform.position;          
        }
        else
        {
            transform.position = upperPositions[Random.Range(0,3)].transform.position;
        }

        Instantiate(teleportEffect, transform.position, transform.rotation);

        isUpper = !isUpper;
    }

    private void SpawnSpiders()
    {
        spidersList.Add(Instantiate(spider, new Vector3(spiderSpawnPoint.transform.position.x, spiderSpawnPoint.transform.position.y - 0.5f),
            spiderSpawnPoint.transform.rotation));
        Instantiate(teleportEffect, new Vector3(spiderSpawnPoint.transform.position.x, spiderSpawnPoint.transform.position.y - 0.5f),
            spiderSpawnPoint.transform.rotation);

    }

    private void RemovePortals()
    {
        isEnraged = true;

        if (isUpper)
        {
            Instantiate(teleportEffect, transform.position, transform.rotation);
            animator.SetTrigger("teleport");
            transform.position = bottomPosition.transform.position;
        }

        Instantiate(teleportEffect, transform.position, transform.rotation);

        isUpper = !isUpper;


        Instantiate(portalsRemoveEffect, new Vector3(portals.transform.position.x, portals.transform.position.y - 0.5f), portals.transform.rotation);
        portals.SetActive(false);

        spider.moveSpeed = spidersEnragedMoveSpeed;
        spider.GetComponent<Rigidbody2D>().mass = 0.001f;
        foreach(var spawnedSpider in spidersList)
        {
            spawnedSpider.moveSpeed = spidersEnragedMoveSpeed;
        }

        CancelInvoke();
        InvokeRepeating("SpawnSpiders", 0, enragedSpawnRepeatingTime);
        shooting.ChangeRepeatingTime(enragedCastingCooldown);
    }
}
