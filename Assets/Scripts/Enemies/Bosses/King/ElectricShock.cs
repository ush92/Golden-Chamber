using UnityEngine;

public class ElectricShock : MonoBehaviour
{
    public bool isPlus;
    public int damage;
    private float damageDelayTimer;

    public GameObject sparksEffect;

    private void FixedUpdate()
    {
        if (damageDelayTimer >= 0)
        {
            damageDelayTimer -= Time.deltaTime;
        }

        if (damageDelayTimer < 0) damageDelayTimer = 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            var polarityDebuff = other.GetComponentInChildren<PolarityDebuff>();

            if (polarityDebuff != null && polarityDebuff.isPlus != this.isPlus && damageDelayTimer == 0)
            {
                other.GetComponent<PlayerHPController>().DamagePlayer(damage);
                Instantiate(sparksEffect, other.transform.position, other.transform.rotation);
            }
        }
    }

    public void Activate(float damageDelay)
    {
        damageDelayTimer = damageDelay;
    }
}
