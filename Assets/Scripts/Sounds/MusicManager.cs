using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource primaryTheme;
    public AudioSource secondaryTheme;

    public PlayerController player;

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

    private void Update()
    {
        if (player != null && !player.isBossEncounter && primaryTheme.isPlaying == false)
        {
            SwitchToPrimaryTheme();
        }

        if (player != null && player.isBossEncounter && secondaryTheme.isPlaying == false)
        {
            SwitchToSecondaryTheme();
        }
    }

    public void SwitchToPrimaryTheme()
    {
        if (primaryTheme != null)
        {
            secondaryTheme.Stop();
            primaryTheme.Play();

            GetComponent<Animator>().Play("MusicFadeIn");
        }
        else
        {
            Debug.Log("Zmiana muzyki jest niemozliwa. Sprawdz czy music manager ma pierwszy klip");
        }
    }

    public void SwitchToSecondaryTheme()
    {
        if(secondaryTheme != null)
        {
            primaryTheme.Pause();
            secondaryTheme.Play();

            GetComponent<Animator>().SetTrigger("FadeOut");
        }
        else
        {
            Debug.Log("Zmiana muzyki jest niemozliwa. Sprawdz czy music manager ma drugi klip");
        }
    }
}
