using System.Collections.Generic;
using UnityEngine;

public class DarkBossBehaviour : MonoBehaviour
{
    public BossActivation activationArea;
    public float startDelay;
    public Animator animator;
    public Transform basePosition;
    public Transform bottomPosition;
    public List<Transform> upperPositions;
    private bool isUpper;

    public float teleportRepeatingTime;
    public GameObject teleportEffect;

    public Transform spiderSpawnPoint;
    public float spawnRepeatingTime;
    public List<EnemyPatrol> spidersList;
    public EnemyPatrol spider;
    public float spidersNormalMoveSpeed;
    public float spidersEnragedMoveSpeed;
    public List<SpriteRenderer> spidersCounterIndicators;

    public GameObject portals;
    public GameObject portalsRemoveEffect;

    public GameObject enrageTimer;
    public float enrageTime;
    public int spidersEnragedCount;
    private bool isEnraged;
    public EnemyBasicShoot shooting;
    public float enragedSpawnRepeatingTime;
    public float enragedCastingCooldown;

    private void OnEnable()
    {
        isUpper = true;
        isEnraged = false;
        enrageTimer.SetActive(true);
        spider.moveSpeed = spidersNormalMoveSpeed;
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
        if(spidersList.Count > spidersEnragedCount && !isEnraged)
        {
            isEnraged = true;
            Invoke("RemovePortals", 0);
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
        foreach(var spawnedSpider in spidersList)
        {
            spawnedSpider.moveSpeed = spidersEnragedMoveSpeed;
        }

        CancelInvoke();
        InvokeRepeating("SpawnSpiders", 0, enragedSpawnRepeatingTime);
        shooting.ChangeRepeatingTime(enragedCastingCooldown);
    }

    private void OnDestroy()
    {
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
}
