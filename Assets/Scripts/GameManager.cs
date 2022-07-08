using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;

    public PlayerController activePlayer;

    public GameObject playerSpawnEffect;
    public Checkpoint currentCheckPoint;

    public Camera playerCamera;

    private static bool isNewGame;
    private static string profileName;

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
        
        saveData.playerData.playerScore  = activePlayer.GetComponentInChildren<ScoreManager>().score;
        saveData.playerData.maxHP = activePlayer.GetComponentInChildren<PlayerHPController>().maxHP;
        saveData.playerData.currentHP = activePlayer.GetComponentInChildren<PlayerHPController>().currentHP;

        //foreach (Enemy enemy in _enemies)
        //{
        //    enemy.PopulateSaveData(saveData);
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
        }
        else
        {
            activePlayer.GetComponentInChildren<ScoreManager>().score = saveData.playerData.playerScore;
            activePlayer.GetComponentInChildren<PlayerHPController>().maxHP = saveData.playerData.maxHP;
            activePlayer.GetComponentInChildren<PlayerHPController>().currentHP = saveData.playerData.currentHP;
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
