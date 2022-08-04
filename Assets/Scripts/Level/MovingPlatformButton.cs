using UnityEngine;

public class MovingPlatformButton : MonoBehaviour
{
    public Sprite turnedOnImage;
    public MovingPlatform platform;
    private bool isPushed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER) && !isPushed)
        {
            isPushed = true;
            platform.isTurnedOn = true;
            GetComponent<SpriteRenderer>().sprite = turnedOnImage;

            if (GameManager.isSoundsOn)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
            }
        }
    }
}
