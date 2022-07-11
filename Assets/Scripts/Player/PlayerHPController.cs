using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UIElements;

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

    public float invincibilityTime;
    public float invincibilityCounter;
    public float hpBarFlashTime = 0.2f;
    private float hpBarFlashCounter;

    private float diff; //to fix hp bar scaling

    void Start()
    {
        currentHP = maxHP;
        diff = transform.localScale.x / hpBar.localScale.x;

        damagePopupBasePosition = damagePopup.transform.localPosition.y;
    }

    void Update()
    {
        if(invincibilityCounter > 0)
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

    public void DamagePlayer(int damage)
    {
        if (invincibilityCounter <= 0)
        {
            currentHP -= damage;
            damagePopup.text = "-" + damage.ToString();
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
            }
        }
    }
}
