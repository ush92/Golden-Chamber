using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{   
    [System.Serializable]
    public struct PlayerData
    {
        public List<bool> levelList;
        //public List<Fruits> fruitRecords;      
    }

    public PlayerData playerData;

    public SaveData()
    {
        playerData.levelList = new List<bool>(new bool[15]);
        //playerData.fruitRecords = new List<Fruits>();

        //for(int i = 0; i <= 14; i++)
        //{
        //    playerData.fruitRecords.Add(new Fruits(0,0));
        //    for (int j = 0; j <= 8; j++)
        //    {
        //        playerData.fruitRecords[i].Add(0);
        //    }
        //}    
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
