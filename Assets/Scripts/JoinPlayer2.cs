using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinPlayer2 : MonoBehaviour
{
    public GameObject playerToLoad;
    private bool isPlayerLoaded;

    void Start()
    {
        
    }

    void Update()
    {
        if(!isPlayerLoaded && GameManager.instance.activePlayers.Count < GameManager.instance.maxPlayers)
        {
            if(Keyboard.current.jKey.wasPressedThisFrame || Keyboard.current.lKey.wasPressedThisFrame || Keyboard.current.rightShiftKey.wasPressedThisFrame
              || Keyboard.current.iKey.wasPressedThisFrame || Keyboard.current.kKey.wasPressedThisFrame)
            {
                Instantiate(playerToLoad, transform.position, transform.rotation);
                isPlayerLoaded = true;
            }
        }
    }
}
