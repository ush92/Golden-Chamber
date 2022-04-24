using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public List<Button> defaultButtons;
    public List<GameObject> screens;

    void Start()
    {
        defaultButtons[0].Select();
    }

    public void Main_NewGame()
    {
        screens[0].gameObject.SetActive(false);
        screens[1].gameObject.SetActive(true);
        defaultButtons[1].Select();
    }

    public void Main_Quit()
    {
        Application.Quit();
    }

    public void NewGame_OnePlayer()
    {
        GameManager.onePlayerMode = true;
        SceneManager.LoadScene("LevelMap");
    }

    public void NewGame_TwoPlayers()
    {
        GameManager.onePlayerMode = false;
        SceneManager.LoadScene("LevelMap");
    }

    public void NewGame_Back()
    {
        screens[1].gameObject.SetActive(false);
        screens[0].gameObject.SetActive(true);
        defaultButtons[0].Select();
    }
}
