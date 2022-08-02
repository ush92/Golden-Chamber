using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public void ManageMusic()
    {
        if (GameManager.isMusicOn || GameManager.profileName == "") //don't mute music if no profile is chosen
        {
            GetComponent<AudioSource>().mute = false;
        }
        else
        {
            GetComponent<AudioSource>().mute = true;
        }
    }
}
