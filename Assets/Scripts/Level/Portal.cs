using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform exitPoint;
    public GameObject teleportEffect;

    void Start()
    {
        
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            Instantiate(teleportEffect, transform.position, transform.rotation);       
            other.transform.position = exitPoint.position;        
            Instantiate(teleportEffect, exitPoint.position, exitPoint.rotation);
        }
    }
}
