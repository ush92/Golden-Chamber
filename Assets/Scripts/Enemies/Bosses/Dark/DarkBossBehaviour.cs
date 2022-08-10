using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static IceBossBehaviour;

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
    public List<GameObject> spidersList;
    public GameObject spider;

    public GameObject portals;
    public float portalRemoveTime;
    public GameObject portalsRemoveEffect;

    void Start()
    {
        isUpper = true;
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
    }
}
