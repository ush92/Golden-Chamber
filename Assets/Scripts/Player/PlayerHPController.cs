using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHPController : MonoBehaviour
{
    public int maxHP = 9;
    public int currentHP;

    public SpriteRenderer[] hpDisplay;
    public Sprite fullHPIcon, emptyHPIcon;

    public Transform hpBar;

    public PlayerController player;

    public float invincibilityTime;
    private float invincibilityCounter;
    public float hpBarFlashTime = 0.2f;
    private float hpBarFlashCounter;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            hpBarFlashCounter -= Time.deltaTime;
            if(hpBarFlashCounter < 0)
            {
                hpBarFlashCounter = hpBarFlashTime;
                hpBar.gameObject.SetActive(!hpBar.gameObject.activeInHierarchy);
            }
        }
        else
        {
            hpBar.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        hpBar.localScale = transform.localScale;
    }

    public void UpdateHPDisplay()
    {
        if(currentHP > ((float)maxHP * 2f) / 3f)
        {
            hpDisplay[0].sprite = fullHPIcon;
            hpDisplay[1].sprite = fullHPIcon;
            hpDisplay[2].sprite = fullHPIcon;
        }
        else if(currentHP > (float)maxHP / 3f && currentHP <= ((float)maxHP * 2f) / 3f)
        {
            hpDisplay[0].sprite = fullHPIcon;
            hpDisplay[1].sprite = fullHPIcon;
            hpDisplay[2].sprite = emptyHPIcon;
        }
        else if(currentHP > 0 && currentHP <= (float)maxHP / 3f)
        {
            hpDisplay[0].sprite = fullHPIcon;
            hpDisplay[1].sprite = emptyHPIcon;
            hpDisplay[2].sprite = emptyHPIcon;
        }
        else
        {
            hpDisplay[0].sprite = emptyHPIcon;
            hpDisplay[1].sprite = emptyHPIcon;
            hpDisplay[2].sprite = emptyHPIcon;
        }
    }

    public void DamagePlayer(int damage)
    {
        if (invincibilityCounter <= 0)
        {
            currentHP -= damage;

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
