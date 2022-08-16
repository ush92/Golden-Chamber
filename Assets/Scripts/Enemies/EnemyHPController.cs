using UnityEngine;
using UnityEngine.UI;

public class EnemyHPController : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    private bool isDeath = false;

    public GameObject enemyDeathEffect;

    public Color flashOnHit;
    public float hitFlashTime;
    private float hitFlashCounter;

    public Transform hpBar;
    public float hpBarFlashTime;
    private float hpBarFlashCounter;
    private float diff; //to fix hp bar scaling

    public Text damagePopup;
    private float damagePopupBasePosition;
    public float damagePopupTime;
    public float damagePopupCounter;
    private float randomOffset;

    public AudioSource enemyDeathSound;

    void Start()
    {
        currentHP = maxHP;
        diff = transform.localScale.x / hpBar.localScale.x;

        if (damagePopup != null)
        {
            damagePopupBasePosition = damagePopup.transform.localPosition.y;
        }
    }

    void Update()
    {
        if (currentHP <= 0)
        {
            if (!isDeath)
            {
                if (name.Equals(Consts.TARGET_DUMMY))
                {
                    Instantiate(enemyDeathEffect, transform.position, transform.rotation);
                    ResetHP();
                }
                else if (name.Contains("DarkBossSpider"))
                {
                    Instantiate(enemyDeathEffect, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
                else
                {
                    isDeath = true;

                    if (GameManager.isSoundsOn)
                    {
                        enemyDeathSound.Play();
                    }

                    Instantiate(enemyDeathEffect, transform.position, transform.rotation);
                    DeactivateAndDestroy();
                }
            }
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

            if (damagePopup != null)
            {
                if (damagePopupCounter == damagePopupTime)
                {
                    damagePopup.transform.localPosition = new Vector3(damagePopup.transform.localPosition.x, damagePopupBasePosition);
                    damagePopupCounter -= Time.deltaTime;
                }
                else if (damagePopupCounter > 0)
                {
                    damagePopupCounter -= Time.deltaTime;

                    damagePopup.transform.localScale = new Vector3(transform.localScale.x, damagePopup.transform.localScale.y);
                    damagePopup.transform.localPosition = new Vector3(damagePopup.transform.localPosition.x + randomOffset, damagePopup.transform.localPosition.y + 1.0f * Time.deltaTime);
                }
                else
                {
                    damagePopupCounter = 0;
                    damagePopup.transform.localPosition = new Vector3(damagePopup.transform.localPosition.x, damagePopupBasePosition);
                    damagePopup.gameObject.SetActive(false);
                }
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
        if (damage > 0)
        {
            if (hitFlashCounter <= 0)
            {
                currentHP -= damage;

                hitFlashCounter = hitFlashTime;
                hpBarFlashCounter = hpBarFlashTime;

                UpdateHPDisplay();
            }

            if (damagePopup != null)
            {
                damagePopup.text = damage.ToString();
                randomOffset = Random.Range(-0.001f, 0.001f);
                damagePopup.gameObject.SetActive(true);
                damagePopupCounter = damagePopupTime;
            }
        }
    }

    private void DeactivateAndDestroy()
    {
        hpBar.gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;     
        foreach (var collider in gameObject.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        foreach (var sprites in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sprites.enabled = false;
        }
        foreach (var otherCollider in gameObject.GetComponentsInChildren<Collider2D>())
        {
            otherCollider.enabled = false;
        }
        foreach (var shooter in gameObject.GetComponents<EnemyBasicShoot>())
        {
            shooter.CancelInvoke();
        }

        Destroy(gameObject, 3.0f);
    }

    public void ResetHP()
    {
        currentHP = maxHP;
    }
}
