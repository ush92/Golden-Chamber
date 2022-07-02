using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;

    private int maxPlayers = 1;
    public List<PlayerController> activePlayers = new List<PlayerController>();

    public GameObject playerSpawnEffect;
    public Checkpoint currentCheckPoint;

    public Camera camera1;

    public static bool onePlayerMode = true;

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

    public void AddPlayer(PlayerController newPlayer)
    {
        if(activePlayers.Count < maxPlayers)
        {
            activePlayers.Add(newPlayer);
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

    private void OnApplicationQuit()
    {
        if (activePlayers.Count > 0)
        {
            SaveJsonData(this);
        }
    }

    #region Save&Load

    public static void SaveJsonData(GameManager _gameManager)
    {
        SaveData saveData = new SaveData();
        _gameManager.PopulateSaveData(saveData);

        if (FileManager.WriteToFile("SaveData01.dat", saveData.SaveToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void PopulateSaveData(SaveData saveData)
    {
        saveData.playerData.playerScore  = activePlayers[0].GetComponentInChildren<ScoreManager>().score;
        saveData.playerData.maxHP = activePlayers[0].GetComponentInChildren<PlayerHPController>().maxHP;
        saveData.playerData.currentHP = activePlayers[0].GetComponentInChildren<PlayerHPController>().currentHP;

        //foreach (Enemy enemy in _enemies)
        //{
        //    enemy.PopulateSaveData(saveData);
        //}
    }

    public static void LoadJsonData(GameManager _gameManager)
    {
        if (FileManager.LoadFromFile("SaveData01.dat", out var json))
        {
            SaveData saveData = new SaveData();
            saveData.LoadFromJson(json);

            _gameManager.LoadFromSaveData(saveData);
            Debug.Log("load complete");
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        activePlayers[0].GetComponentInChildren<ScoreManager>().score = saveData.playerData.playerScore;
        activePlayers[0].GetComponentInChildren<PlayerHPController>().maxHP = saveData.playerData.maxHP;
        activePlayers[0].GetComponentInChildren<PlayerHPController>().currentHP = saveData.playerData.currentHP;
    }
    
    #endregion
}
