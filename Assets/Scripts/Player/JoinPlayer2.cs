using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinPlayer2 : MonoBehaviour
{
    public GameObject playerToLoad;
    private bool isPlayerLoaded;

    public Camera camera2;

    private void Awake()
    {
       camera2 = FindObjectOfType<Camera>();
    }

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
                Instantiate(playerToLoad, GameManager.instance.currentCheckPoint.transform.position, transform.rotation);
                isPlayerLoaded = true;
            }
        }
    }
}
