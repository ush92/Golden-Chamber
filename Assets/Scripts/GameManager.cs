using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxPlayers;
    public List<PlayerController> activePlayers = new List<PlayerController>();

    public GameObject playerSpawnEffect;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddPlayer(PlayerController newPlayer)
    {
        if(activePlayers.Count < maxPlayers)
        {
            activePlayers.Add(newPlayer);
            Instantiate(playerSpawnEffect, newPlayer.transform.position, newPlayer.transform.rotation);
            var camera1 = activePlayers[0].GetComponentInChildren<Camera>();
            camera1.rect = new Rect(0, 0.5f, 1f, 0.5f);
        }
        else
        {
            Destroy(newPlayer.gameObject);
        }
    }
}
