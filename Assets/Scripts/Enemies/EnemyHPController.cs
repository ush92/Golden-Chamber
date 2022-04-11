using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPController : MonoBehaviour
{
    public int HP = 1;

    public GameObject enemyDeathEffect;

    public Color flashOnHit;
    public float hitFlashTime;
    private float hitFlashCounter;

    void Start()
    {
        
    }

    void Update()
    {
        if (HP <= 0)
        {
            Instantiate(enemyDeathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            if (hitFlashCounter > 0)
            {
                hitFlashCounter -= Time.deltaTime;
                GetComponent<Renderer>().material.color = flashOnHit;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    public void takeDamage(int damage)
    {
        HP -= damage;

        hitFlashCounter = hitFlashTime;
    }
}
