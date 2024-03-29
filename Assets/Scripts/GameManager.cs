using System.Collections;
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
    public static string profileName = "";
    public string currentLevel;

    public static List<bool> levelList = new List<bool>(new bool[15]);
    public static List<List<int>> levelRecords = new List<List<int>>();

    private MusicManager musicManager;
    public static bool isMusicOn;
    public static bool isSoundsOn;
    public Animator musicTransition;

    public Animator levelTransition;
    public float transitionTime = 1.0f;

    private void Awake()
    {
        instance = this;

        musicManager = FindObjectOfType<MusicManager>();

        for (int i = 0; i < 13; i++)
        {
            levelRecords.Add(new List<int>());
            for (int j = 0; j <= 9; j++) //0-8 - fruits, 9 - time
            {
                levelRecords[i].Add(0);
            }
        }
    }

    private void Update()
    {
        musicManager.ManageMusic();
    }

    public static void CreateGame(bool _isNewGame, string _profileName)
    {
        isNewGame = _isNewGame;
        profileName = _profileName;
    }

    public void AddPlayer(PlayerController newPlayer)
    {
        if (!activePlayer)
        {
            activePlayer = newPlayer;
            PlayerRespawnEffect();

            if (SceneManager.GetActiveScene().name.Equals(Consts.LEVEL_MAP))
            {
                transform.localPosition = levelMapLastPosition;
            }
            else
            {
                transform.localPosition = newPlayer.transform.localPosition;
            }

            LoadJsonData(instance);
            activePlayer.equipmentManager.currentItem = PlayerPrefs.GetInt(Consts.PLAYER_CURRENT_ITEM);
            activePlayer.equipmentManager.UpdateEquipment();        
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
        levelMapLastPosition = Vector3.zero;
        StartCoroutine(LoadLevel(Consts.MAIN_MENU));
    }

    public void ToggleSoundOptions()
    {
        activePlayer.soundOptionsWindow.gameObject.SetActive(!activePlayer.soundOptionsWindow.activeSelf);
    }

    public void TogglePause()
    {
        activePlayer.pauseWindow.gameObject.SetActive(!activePlayer.pauseWindow.activeSelf);

        if(activePlayer.pauseWindow.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public IEnumerator LoadLevel(string levelName)
    {
        if (activePlayer)
        {
            SaveJsonData(this);
        }

        if (levelName.Equals(Consts.MAIN_MENU))
        {
            Destroy(activePlayer);
            PlayerPrefs.SetInt(Consts.PLAYER_CURRENT_ITEM, 0);
            ResetPlayerData();
        }

        if (activePlayer && activePlayer.pauseWindow.activeSelf)
        {
            activePlayer.pauseWindow.gameObject.SetActive(!activePlayer.pauseWindow.activeSelf);
            Time.timeScale = 1;
        }

        musicTransition.SetTrigger("FadeOut");
        levelTransition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
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
        else
        {
            Debug.Log("Error with saving data. GameManager.SaveJsonData");
        }
    }

    public void PopulateSaveData(SaveData saveData)
    {     
        for (int i = 0; i < 13; i++)
        {
            saveData.playerData.levelList[i] = levelList[i];
        }

        saveData.playerData.isMusicOn = isMusicOn;
        saveData.playerData.isSoundsOn = isSoundsOn;

        #region levelRecords

        saveData.playerData.level0Records = levelRecords[0];
        saveData.playerData.level1Records = levelRecords[1];
        saveData.playerData.level2Records = levelRecords[2];
        saveData.playerData.level3Records = levelRecords[3];
        saveData.playerData.level4Records = levelRecords[4];
        saveData.playerData.level5Records = levelRecords[5];
        saveData.playerData.level6Records = levelRecords[6];
        saveData.playerData.level7Records = levelRecords[7];
        saveData.playerData.level8Records = levelRecords[8];
        saveData.playerData.level9Records = levelRecords[9];
        saveData.playerData.level10Records = levelRecords[10];
        saveData.playerData.level11Records = levelRecords[11];
        saveData.playerData.level12Records = levelRecords[12];

        #endregion
    }

    public static void LoadJsonData(GameManager _gameManager)
    { 
        if (FileManager.LoadFromFile(profileName + ".dat", out var json))
        {
            SaveData saveData = new SaveData();
            saveData.LoadFromJson(json);

            _gameManager.LoadFromSaveData(saveData);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        if (isNewGame)
        {
            isNewGame = false;
            ResetPlayerData();
        }
        else
        {
            for (int i = 0; i < 13; i++)
            {
                levelList[i] = saveData.playerData.levelList[i];
            }

            isMusicOn = saveData.playerData.isMusicOn;
            isSoundsOn = saveData.playerData.isSoundsOn;

            #region levelRecords

            levelRecords[0] = saveData.playerData.level0Records;
            levelRecords[1] = saveData.playerData.level1Records;
            levelRecords[2] = saveData.playerData.level2Records;
            levelRecords[3] = saveData.playerData.level3Records;
            levelRecords[4] = saveData.playerData.level4Records;
            levelRecords[5] = saveData.playerData.level5Records;
            levelRecords[6] = saveData.playerData.level6Records;
            levelRecords[7] = saveData.playerData.level7Records;
            levelRecords[8] = saveData.playerData.level8Records;
            levelRecords[9] = saveData.playerData.level9Records;
            levelRecords[10] =saveData.playerData.level10Records;
            levelRecords[11] =saveData.playerData.level11Records;
            levelRecords[12] =saveData.playerData.level12Records;

            #endregion
        }
    }

    private void ResetPlayerData()
    {
        for (int i = 0; i < 13; i++)
        {
            levelList[i] = false;
        }

        isMusicOn = true;
        isSoundsOn = true;

        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j <= 9; j++)
            {
                levelRecords[i][j] = 0;
            }
        }
    }

    #endregion
}
