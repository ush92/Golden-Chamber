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
    public static List<List<int>> fruitRecords = new List<List<int>>();

    private void Awake()
    {
        instance = this;

        for (int i = 0; i <= 14; i++)
        {
            fruitRecords.Add(new List<int>());
            for (int j = 0; j <= 8; j++)
            {
                fruitRecords[i].Add(0);
            }
        }
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
            Debug.Log("Save successful");
        }
    }

    public void PopulateSaveData(SaveData saveData)
    {     
        for (int i = 0; i <= 14; i++)
        {
            saveData.playerData.levelList[i] = levelList[i];
        }

        //for (int i = 0; i <= 14; i++)
        //{
        //    for (int j = 0; j <= 8; j++)
        //    {
        //        saveData.playerData.fruitRecords[i][j] = fruitRecords[i][j];
        //    }
        //}
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

            for (int i = 0; i <= 14; i++)
            {
                levelList[i] = false;
            }

            for (int i = 0; i <= 14; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    fruitRecords[i][j] = 0;
                }
            }
        }
        else
        {
            for (int i = 0; i <= 14; i++)
            {
                levelList[i] = saveData.playerData.levelList[i];
            }

            //for (int i = 0; i <= 14; i++)
            //{
            //    for (int j = 0; j <= 8; j++)
            //    {
            //        fruitRecords[i][j] = saveData.playerData.fruitRecords[i][j];
            //    }
            //}
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
