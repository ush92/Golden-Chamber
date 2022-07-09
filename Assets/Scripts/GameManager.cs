using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;

    public PlayerController activePlayer;
    public Camera playerCamera;
    public static Vector3 levelMapLastPosition;
    public GameObject playerSpawnEffect;
    public Checkpoint currentCheckPoint;

    private static bool isNewGame;
    private static string profileName;
    public string currentLevel;

    public static List<bool> levelList = new List<bool>(new bool[15]);

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public static void CreateGame(bool _isNewGame, string _profileName)
    {
        isNewGame = _isNewGame;
        profileName = _profileName;
    }

    public void AddPlayer(PlayerController newPlayer)
    {
        if(!activePlayer)
        {
            activePlayer = newPlayer;
            PlayerRespawnEffect();
        }
        else
        {
            Destroy(newPlayer.gameObject);
        }
    }

    public void PlayerRespawnEffect()
    {
        Instantiate(playerSpawnEffect, currentCheckPoint.transform.position, currentCheckPoint.transform.rotation);
    }

    public void SetNewCheckpoint(Checkpoint newCheckPoint)
    {
        if (newCheckPoint != currentCheckPoint)
        { 
            currentCheckPoint = newCheckPoint;
        }
    }

    public void BackToMainMenu()
    {
        if (activePlayer)
        {
            SaveJsonData(this);
        }

        levelMapLastPosition = Vector3.zero;
        SceneManager.LoadScene(Consts.MAIN_MENU);
    }

    #region Save&Load

    public static void SaveJsonData(GameManager _gameManager)
    {     
        SaveData saveData = new SaveData();
        _gameManager.PopulateSaveData(saveData);

        if (FileManager.WriteToFile(profileName + ".dat", saveData.SaveToJson()))
        {
            //Debug.Log("Save successful");
        }
    }

    public void PopulateSaveData(SaveData saveData)
    {     
        //saveData.playerData.maxHP = activePlayer.GetComponentInChildren<PlayerHPController>().maxHP;

        //0 level1_1; 1 level1_2; 2 level1_3; 3 level1_4; 4 level2_1; 5 level2_2; 6 level2_3; 7 level3_1;
        //8 level3_2; 9 level3_3; 10 level3_4; 11 level4_1; 12 level4_2; 13 level4_3; 14 level5_1;
        for (int i = 0; i <= 14; i++)
        {
            saveData.playerData.levelList[i] = levelList[i];
        }
    }

    public static void LoadJsonData(GameManager _gameManager)
    {
        if (FileManager.LoadFromFile(profileName + ".dat", out var json))
        {
            SaveData saveData = new SaveData();
            saveData.LoadFromJson(json);

            _gameManager.LoadFromSaveData(saveData);
            Debug.Log("load complete");
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        if (isNewGame)
        {
            isNewGame = false;

            //0 level1_1; 1 level1_2; 2 level1_3; 3 level1_4; 4 level2_1; 5 level2_2; 6 level2_3; 7 level3_1;
            //8 level3_2; 9 level3_3; 10 level3_4; 11 level4_1; 12 level4_2; 13 level4_3; 14 level5_1;
            for (int i = 0; i <= 14; i++)
            {
                levelList[i] = false;
            }
        }
        else
        {
            //saveData.playerData.maxHP = activePlayer.GetComponentInChildren<PlayerHPController>().maxHP;

            for (int i = 0; i <= 14; i++)
            {
                levelList[i] = saveData.playerData.levelList[i];
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (activePlayer)
        {
            SaveJsonData(this);
        }
    }
    private void OnApplicationPause()
    {
        if (activePlayer)
        {
            SaveJsonData(this);
        }
    }
    private void OnApplicationFocus()
    {
        if (activePlayer)
        {
            SaveJsonData(this);
        }
    }

    #endregion
}
