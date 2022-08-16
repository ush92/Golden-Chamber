using UnityEngine;

public class DrainLife : MonoBehaviour
{
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
                enemyHp.RegenHP(drainAmount);
            }
        }
    }
}
