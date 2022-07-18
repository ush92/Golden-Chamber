using UnityEngine;

public class EnemyHPController : MonoBehaviour
{
    public int maxHP;
    public int currentHP;

    public GameObject enemyDeathEffect;

    public Color flashOnHit;
    public float hitFlashTime;
    private float hitFlashCounter;

    public Transform hpBar;
    public float hpBarFlashTime;
    private float hpBarFlashCounter;
    private float diff; //to fix hp bar scaling

    void Start()
    {
        currentHP = maxHP;
        diff = transform.localScale.x / hpBar.localScale.x;
    }

    void Update()
    {
        if (currentHP <= 0)
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

            if (hpBarFlashCounter > 0)
            {
                hpBarFlashCounter -= Time.deltaTime;
                hpBar.gameObject.SetActive(true);
            }
            else
            {
                hpBar.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHPDisplay()
    {
        hpBar.Find("Bar").localScale = new Vector3(((float)currentHP / (float)maxHP) * 5, 1f);
    }

    private void LateUpdate()
    {
        hpBar.localScale = new Vector3(transform.localScale.x / diff, hpBar.localScale.y);
    }

    public void takeDamage(int damage)
    {
        if (hitFlashCounter <= 0)
        {
            currentHP -= damage;

            hitFlashCounter = hitFlashTime;
            hpBarFlashCounter = hpBarFlashTime;

            UpdateHPDisplay();
        }
    }

    public void ResetHP()
    {
        currentHP = maxHP;
    }
}
