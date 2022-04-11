using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPController : MonoBehaviour
{
    public int HP = 1;

    public GameObject enemyDeathEffect;

    void Start()
    {
        
    }

    void Update()
    {
        if(HP <= 0)
        {
            Instantiate(enemyDeathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        HP -= damage;
    }
}
