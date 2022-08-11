using UnityEngine;

public class DrainLife : MonoBehaviour
{
    public GameObject drainLifeEffect;
    public string enemyName;
    private EnemyHPController enemyHp;
    public int drainAmount;
    private bool isDrained = false;

    private void Start()
    {
        enemyHp = GameObject.Find(enemyName).GetComponent<EnemyHPController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER) && !isDrained)
        {
            isDrained = true;

            if (enemyHp != null)
            {
                Instantiate(drainLifeEffect, enemyHp.transform.position, Quaternion.Euler(0, 0, 0));

                enemyHp.currentHP += drainAmount;
                if (enemyHp.currentHP > enemyHp.maxHP)
                {
                    enemyHp.currentHP = enemyHp.maxHP;
                }
            }
        }
    }
}
