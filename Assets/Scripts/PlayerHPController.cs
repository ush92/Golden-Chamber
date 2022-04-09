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

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        if(Keyboard.current.gKey.wasPressedThisFrame)
        {
            UpdateHPDisplay();
        }
    }

    public void UpdateHPDisplay()
    {
        if(currentHP >= maxHP * 0.67f)
        {
            hpDisplay[0].sprite = fullHPIcon;
            hpDisplay[1].sprite = fullHPIcon;
            hpDisplay[2].sprite = fullHPIcon;
        }
        else if(currentHP >= maxHP * 0.33f && currentHP < maxHP * 0.67f)
        {
            hpDisplay[0].sprite = fullHPIcon;
            hpDisplay[1].sprite = fullHPIcon;
            hpDisplay[2].sprite = emptyHPIcon;
        }
        else if(currentHP > 0 && currentHP < maxHP * 0.33f)
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

    public void DamagePlayer()
    {

    }
}
