using UnityEngine;
using UnityEngine.UI;

public class SoundWindow : MonoBehaviour
{
    public Image buttonMusicImage;
    public Image buttonSoundsImage;

    private void Start()
    {
        RefreshSoundButtonImages();
    }

    public void ButtonMusicClick()
    {
        GameManager.isMusicOn = !GameManager.isMusicOn;
        RefreshSoundButtonImages();
    }

    public void ButtonSoundsClick()
    {
        GameManager.isSoundsOn = !GameManager.isSoundsOn;
        RefreshSoundButtonImages();
    }

    private void RefreshSoundButtonImages()
    {
        if (GameManager.isMusicOn)
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
}
