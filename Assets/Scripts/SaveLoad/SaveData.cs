using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{   
    [System.Serializable]
    public struct PlayerData
    {
        //config
        public bool isMusicOn;
        public bool isSoundsOn;
        
        public List<bool> levelList; //completed or not

        public List<int> level0Records; //fruits and time
        public List<int> level1Records;
        public List<int> level2Records;
        public List<int> level3Records;
        public List<int> level4Records;
        public List<int> level5Records;
        public List<int> level6Records;
        public List<int> level7Records;
        public List<int> level8Records;
        public List<int> level9Records;
        public List<int> level10Records;
        public List<int> level11Records;
        public List<int> level12Records;
    }

    public PlayerData playerData;

    public SaveData()
    {
        playerData.isMusicOn = true;
        playerData.isSoundsOn = true;

        playerData.levelList = new List<bool>(new bool[13]);

        playerData.level0Records = new List<int>(new int[10]);
        playerData.level1Records = new List<int>(new int[10]);
        playerData.level2Records = new List<int>(new int[10]);
        playerData.level3Records = new List<int>(new int[10]);
        playerData.level4Records = new List<int>(new int[10]);
        playerData.level5Records = new List<int>(new int[10]);
        playerData.level6Records = new List<int>(new int[10]);
        playerData.level7Records = new List<int>(new int[10]);
        playerData.level8Records = new List<int>(new int[10]);
        playerData.level9Records = new List<int>(new int[10]);
        playerData.level10Records = new List<int>(new int[10]);
        playerData.level11Records = new List<int>(new int[10]);
        playerData.level12Records = new List<int>(new int[10]);
    }

    public string SaveToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string data)
    {
        JsonUtility.FromJsonOverwrite(data, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData saveData);
    void LoadFromSaveData(SaveData saveData);
}
