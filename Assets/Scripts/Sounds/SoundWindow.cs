using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundWindow : MonoBehaviour
{
    public Image buttonMusicImage;
    public Image buttonSoundsImage;

    void Update()
    {
        if(GameManager.isMusicOn)
        {
            buttonMusicImage.color = new Color32(225, 255, 255, 255);
        }
        else
        {
            buttonMusicImage.color = new Color32(255, 255, 255, 100);
        }

        if (GameManager.isSoundsOn)
        {
            buttonSoundsImage.color = new Color32(225, 255, 255, 255);
        }
        else
        {
            buttonSoundsImage.color = new Color32(255, 255, 255, 100);
        }
    }

    public void ButtonMusicClick()
    {
        GameManager.isMusicOn = !GameManager.isMusicOn;
    }

    public void ButtonSoundsClick()
    {
        GameManager.isSoundsOn = !GameManager.isSoundsOn;
    }
}
