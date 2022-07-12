using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{   
    [System.Serializable]
    public struct PlayerData
    {
        public List<bool> levelList;

        //todo: add save fruits and time
    }
    
    public PlayerData playerData;

    public SaveData()
    {
        playerData.levelList = new List<bool>(new bool[15]);
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
