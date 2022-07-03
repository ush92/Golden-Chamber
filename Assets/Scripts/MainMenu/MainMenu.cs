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
        
    }

    public void Main_Continue()
    {
        GameManager.SetNewGameFlag(false);
        SceneManager.LoadScene(Consts.LEVEL_MAP);
    }

    public void Main_NewGame()
    {
        GameManager.SetNewGameFlag(true);
        SceneManager.LoadScene(Consts.LEVEL_MAP);
    }

    public void Main_Quit()
    {
        Application.Quit();
    }

    //public void NewGame_Back()
    //{
    //    screens[1].gameObject.SetActive(false);
    //    screens[0].gameObject.SetActive(true);
    //    defaultButtons[0].Select();
    //}
}
