using System.Collections.Generic;
using UnityEngine;

public class IceBossBehaviour : MonoBehaviour
{
    public Animator animator;

    public List<Transform> teleportPoints;
    private Side currentPoint;
    public float teleportRepeatingTime;
    public GameObject teleportEffect;

    public GameObject frozenGhost;
    public List<GameObject> frozenGhostList;
    public float spawnRepeatingTime;
    private Side spawnCheck;

    public EnemyBasicShoot casting;
    public float slowCasting;
    public float fastCasting;

    public float startDelay;

    enum Side { Upper_Right, Bottom_Left, Upper_Left, Bottom_Right };

    void OnEnable()
    {
        currentPoint = Side.Upper_Right;
        InvokeRepeating("Teleport", startDelay, teleportRepeatingTime);
        InvokeRepeating("SpawnGhosts", startDelay, spawnRepeatingTime);
    }

    void Teleport()
    {
        if (currentPoint == Side.Bottom_Left || currentPoint == Side.Bottom_Right)
        {
            casting.ChangeRepeatingTime(slowCasting);
        }
        else
        {
            casting.ChangeRepeatingTime(fastCasting);
        }
        
        Instantiate(teleportEffect, transform.position, transform.rotation);
        transform.position = teleportPoints[(int)currentPoint].transform.position;

        if (currentPoint == Side.Bottom_Left || currentPoint == Side.Upper_Left)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        animator.SetTrigger("teleport");

        spawnCheck = currentPoint;
        int tmpIndex = (int)currentPoint;      
        while (tmpIndex == (int)currentPoint)
        {
            tmpIndex = Random.Range(0, 4);
        }
        currentPoint = (Side)tmpIndex;
    }

    private void SpawnGhosts()
    {
        if (spawnCheck == Side.Upper_Right || spawnCheck == Side.Upper_Left)
        {
            Spawn(Side.Bottom_Left);
            Spawn(Side.Bottom_Right);
        }
        else if (spawnCheck == Side.Bottom_Left)
        {
            Spawn(Side.Bottom_Right);
        }
        else
        {
            Spawn(Side.Bottom_Left);
        }
    }

    private void Spawn(Side position)
    {
        frozenGhostList.Add(Instantiate(frozenGhost, new Vector3(teleportPoints[(int)position].transform.position.x, teleportPoints[(int)position].transform.position.y - 0.5f),
            teleportPoints[(int)position].transform.rotation));
        Instantiate(teleportEffect, new Vector3(teleportPoints[(int)position].transform.position.x, teleportPoints[(int)position].transform.position.y - 0.5f),
            teleportPoints[(int)position].transform.rotation);

    }

    void OnDestroy()
    {
        foreach(var ghost in frozenGhostList)
        {
            Destroy(ghost);
        }
    }
}
