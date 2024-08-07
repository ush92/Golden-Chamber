using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHPController : MonoBehaviour
{
    public int maxHP;
    public int currentHP;

    public Transform hpBar;
    public Text hpText;

    public Text damagePopup;
    private float damagePopupBasePosition;
    public float damagePopupTime;
    public float damagePopupCounter;

    public PlayerController player;
    public GameObject playerDamageEffect;
    public GameObject playerFreezeEffect;

    public float invincibilityTime;
    public float invincibilityCounter;
    public float hpBarFlashTime = 0.2f;
    private float hpBarFlashCounter;

    private float diff; //to fix hp bar scaling
    private bool isDataLoaded = false;

    void Start()
    {
        currentHP = maxHP;
        diff = transform.localScale.x / hpBar.localScale.x;

        damagePopupBasePosition = damagePopup.transform.localPosition.y;
    }

    void Update()
    {
        if (!isDataLoaded)
        {
            isDataLoaded = true;
            if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL2_3)] == true)
            {
                maxHP = 10;
                currentHP = maxHP;
            }
        }

        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            hpBarFlashCounter -= Time.deltaTime;

            if (hpBarFlashCounter < 0)
            {
                hpBarFlashCounter = hpBarFlashTime;
                hpBar.gameObject.SetActive(!hpBar.gameObject.activeInHierarchy);
            }
        }
        else
        {
            hpBar.gameObject.SetActive(false);
        }

        if (damagePopupCounter > 0)
        {
            damagePopupCounter -= Time.deltaTime;

            damagePopup.transform.localScale = new Vector3(transform.localScale.x, damagePopup.transform.localScale.y);
            damagePopup.transform.localPosition = new Vector3(damagePopup.transform.localPosition.x, damagePopup.transform.localPosition.y + 1.0f * Time.deltaTime);
        }
        else
        {
            damagePopup.transform.localPosition = new Vector3(damagePopup.transform.localPosition.x, damagePopupBasePosition);
            damagePopup.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        hpBar.localScale = new Vector3(transform.localScale.x / diff, hpBar.localScale.y);
        hpText.text = currentHP.ToString();
    }

    public void UpdateHPDisplay()
    {
        hpBar.Find("Bar").localScale = new Vector3(((float)currentHP / (float)maxHP) * 5, 1f);
    }

    public void DamagePlayer(int damage, string damagerName = "")
    {
        if (invincibilityCounter <= 0)
        {            
            currentHP -= damage;
            damagePopup.text = damage.ToString();
            damagePopup.gameObject.SetActive(true);
            damagePopupCounter = damagePopupTime;

            if (currentHP < 0)
            {
                currentHP = 0;
            }

            UpdateHPDisplay();

            invincibilityCounter = invincibilityTime;

            if (currentHP == 0)
            {
                player.Death();
                hpBar.gameObject.SetActive(false);
                invincibilityCounter = 0;

                player.SoundEffect("Death");
            }
            else
            {
                OnHitEffects(damagerName);
            }
        }
    }

    private void OnHitEffects(string damagerName = "")
    {
        if (SceneManager.GetActiveScene().name.Equals(Consts.LEVEL3_2) || damagerName == "GoldenWraith" || damagerName.Contains("spikeIcicle"))
        {
            Instantiate(playerFreezeEffect, player.transform.position, player.transform.rotation);
            player.isFrozen = true;
            player.SoundEffect("Frozen", 1.0f, 1.5f);
        }
        else
        {
            Instantiate(playerDamageEffect, player.transform.position, player.transform.rotation);
            player.SoundEffect($"Hurt{Random.Range(1, 3)}", 1.0f, 1.5f);
        }
    }

    public void HealPlayer(int hp)
    {
        currentHP += hp;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
}
