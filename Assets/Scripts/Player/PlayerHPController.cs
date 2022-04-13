using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHPController : MonoBehaviour
{
    public int maxHP;
    public int currentHP;

    public Transform hpBar;

    public PlayerController player;

    public float invincibilityTime;
    public float invincibilityCounter;
    public float hpBarFlashTime = 0.2f;
    private float hpBarFlashCounter;

    private float diff; //fix hp bar scaling

    void Start()
    {
        currentHP = maxHP;

        diff = transform.localScale.x / hpBar.localScale.x;
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
        hpBar.localScale = new Vector3(transform.localScale.x / diff, hpBar.localScale.y);     
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
