using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource Jump1;
    public AudioSource Jump2;
    public AudioSource Jump3;

    public AudioSource Frozen;

    public AudioSource Swoosh1;
    public AudioSource Swoosh2;
    public AudioSource Swoosh3;
    public AudioSource Axe1;
    public AudioSource Axe2;
    public AudioSource Axe3;
    public AudioSource Stone1;
    public AudioSource Stone2;
    public AudioSource Stone3;
    public AudioSource Fire1;
    public AudioSource Fire2;
    public AudioSource Fire3;
    public AudioSource Arctic1;
    public AudioSource DarkBlade1;
    public AudioSource DarkBlade2;
    public AudioSource DarkBlade3;
    public AudioSource Poison1;
    public AudioSource Poison2;
    public AudioSource Poison3;
    public AudioSource GoldenAxe1;
    public AudioSource GoldenAxe2;
    public AudioSource GoldenAxe3;

    public AudioSource GetSound(string name)
    {
        switch (name)
        {
            case "Jump1":
                return Jump1;
            case "Jump2":
                return Jump2;
            case "Jump3":
                return Jump3;
            case "Frozen":
                return Frozen;
            case "Swoosh1":
                return Swoosh1;
            case "Swoosh2":
                return Swoosh2;
            case "Swoosh3":
                return Swoosh3;
            case "Axe1":
                return Axe1;
            case "Axe2":
                return Axe2;
            case "Axe3":
                return Axe3;
            case "Stone1":
                return Stone1;
            case "Stone2":
                return Stone2;
            case "Stone3":
                return Stone3;
            case "Fire1":
                return Fire1;
            case "Fire2":
                return Fire2;
            case "Fire3":
                return Fire3;
            case "Arctic1":
                return Arctic1;
            case "DarkBlade1":
                return DarkBlade1;
            case "DarkBlade2":
                return DarkBlade2;
            case "DarkBlade3":
                return DarkBlade3;
            case "Poison1":
                return Poison1;
            case "Poison2":
                return Poison2;
            case "Poison3":
                return Poison3;
            case "GoldenAxe1":
                return GoldenAxe1;
            case "GoldenAxe2":
                return GoldenAxe2;
            case "GoldenAxe3":
                return GoldenAxe3;

            default:
                Debug.Log("Wrong sound name in PlayerSounds.GetSound");
                return null;
        }
    }
}
