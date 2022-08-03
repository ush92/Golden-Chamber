using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource primaryTheme;
    public AudioSource secondaryTheme;

    public void ManageMusic()
    {
        if (GameManager.isMusicOn || GameManager.profileName == "") //don't mute music if no profile is chosen e.g in main menu on app first run
        {
            foreach (var audio in GetComponents<AudioSource>())
            {
                audio.mute = false;
            }
        }
        else
        {
            foreach (var audio in GetComponents<AudioSource>())
            {
                audio.mute = true;
            }
        }
    }
    public void SwitchToSecondaryTheme()
    {
        if(secondaryTheme != null)
        {
            primaryTheme.Pause();
            secondaryTheme.Play();
        }
        else
        {
            Debug.Log("Zmiana muzyki jest niemozliwa. Sprawdz czy music manager ma drugi klip");
        }
    }

    public void SwitchToPrimaryTheme()
    {
        secondaryTheme.Stop();
        primaryTheme.Play();
    }
}
