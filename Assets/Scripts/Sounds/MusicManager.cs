using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public void ManageMusic()
    {
        if (GameManager.isMusicOn)
        {
            GetComponent<AudioSource>().mute = false;
        }
        else
        {
            GetComponent<AudioSource>().mute = true;
        }
    }
}
