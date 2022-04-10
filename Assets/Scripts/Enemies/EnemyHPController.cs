using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPController : MonoBehaviour
{
    public int HP = 1;

    void Start()
    {
        
    }

    void Update()
    {
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        HP -= damage;
    }
}
