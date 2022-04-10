using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damageToDeal = 1;

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
            other.GetComponent<PlayerHPController>().DamagePlayer(damageToDeal);

            var player = other.GetComponent<PlayerController>();
            player.knockbackCounter = player.knockbackLength;
            player.knockbackFromRight = other.transform.position.x < transform.position.x ? true : false;         
        }
    }
}
