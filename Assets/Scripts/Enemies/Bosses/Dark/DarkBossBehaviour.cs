using System.Collections.Generic;
using UnityEngine;

public class DarkBossBehaviour : MonoBehaviour
{
    public BossActivation activationArea;
    public float startDelay;
    public Animator animator;
    public Transform basePosition;
    public Transform bottomPosition;
    private bool isUpper;

    public float teleportRepeatingTime;
    public GameObject teleportEffect;

    public Transform spiderSpawnPoint;
    public float spawnRepeatingTime;
    public List<EnemyPatrol> spidersList;
    public EnemyPatrol spider;
    public float spidersNormalMoveSpeed;
    public float spidersEnragedMoveSpeed;

    public GameObject portals;
    public float portalRemoveTime;
    public GameObject portalsRemoveEffect;

    private bool isEnraged;
    public EnemyBasicShoot shooting;
    public float enragedSpawnRepeatingTime;
    public float enragedCastingCooldown;

    void Start()
    {
        isUpper = true;
        isEnraged = false;
        spider.moveSpeed = spidersNormalMoveSpeed;
        InvokeRepeating("Teleport", startDelay, teleportRepeatingTime);
        InvokeRepeating("SpawnSpiders", startDelay, spawnRepeatingTime);
        Invoke("RemovePortals", portalRemoveTime);
    }

    private void OnEnable()
    {
        
    }

    void Update()
    {
        
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
            transform.position = basePosition.transform.position;
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
        Instantiate(portalsRemoveEffect, new Vector3(portals.transform.position.x, portals.transform.position.y - 0.5f),
            portals.transform.rotation);
        portals.SetActive(false);

        isEnraged = true;
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
        foreach (var spawnedSpider in spidersList)
        {
            Destroy(spawnedSpider.gameObject);
        }
    }
}
