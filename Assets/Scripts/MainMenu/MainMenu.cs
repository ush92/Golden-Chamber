using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public List<GameObject> screens;

    public InputField newProfileInput;
    public Text newProfileErrorLabel;

    public GameObject profilesListPanel;
    public GameObject ProfilesListRowPrefab;

    void Start()
    {
        newProfileInput.onValidateInput += delegate (string s, int i, char c)
        {
            if (s.Length >= 12) { return '\0'; }
            c = char.ToUpper(c);
            return char.IsLetter(c) ? c : '\0';
        };

        
        foreach (var profile in FileManager.GetAllProfiles())
        {
            GameObject row = Instantiate(ProfilesListRowPrefab);
            row.name = profile;
            row.transform.SetParent(profilesListPanel.transform, false);
            row.GetComponent<Button>().onClick.AddListener(() => { Continue_OpenProfile(profile); });
            row.GetComponentInChildren<Text>().text = profile;
        }
    }

    public void Main_Continue()
    {
        screens[2].gameObject.SetActive(true);
        screens[0].gameObject.SetActive(false);
    }

    public void Main_NewGame()
    {
        screens[1].gameObject.SetActive(true);
        screens[0].gameObject.SetActive(false);
    }

    public void Main_Quit()
    {
        Application.Quit();
    }

    public void NewGame_CreateProfile()
    {
        if (newProfileInput.text.Trim().Equals(""))
        {
            newProfileErrorLabel.text = Consts.NEW_PROFILE_ERROR_EMPTY_NAME;
        }
        else if (FileManager.IsFileExist(newProfileInput.text + ".dat"))
        {
            newProfileErrorLabel.text = Consts.NEW_PROFILE_ERROR_ALREADY_USED;
        }
        else
        {
            if (FileManager.WriteToFile(newProfileInput.text + ".dat", ""))
            {
                Debug.Log("Profile created successfully");

                GameManager.CreateGame(_isNewGame: true, newProfileInput.text);
                SceneManager.LoadScene(Consts.LEVEL_MAP);
            }
        }
    }

    public void NewGame_Back()
    {
        newProfileInput.text = "";

        screens[1].gameObject.SetActive(false);
        screens[0].gameObject.SetActive(true);
    }

    public void Continue_OpenProfile(string profileName)
    {
        GameManager.CreateGame(_isNewGame: false, profileName);
        StartCoroutine(GameManager.instance.LoadLevel(Consts.LEVEL_MAP));
        Debug.Log($"Game {profileName} loaded");
    }

    public void Continue_Back()
    {
        screens[2].gameObject.SetActive(false);
        screens[0].gameObject.SetActive(true);
    }
}
