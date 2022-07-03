using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChangeTrigger : MonoBehaviour
{
    public Image background;
    public Sprite newSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER) && background.sprite != newSprite)
        {
            background.sprite = newSprite;
        }
    }
}
