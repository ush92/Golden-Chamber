using UnityEngine;

[System.Serializable]
public class SaveData
{   
    [System.Serializable]
    public struct PlayerData
    {
        public int playerScore;
        public int maxHP;
        public int currentHP;
    }
    
    public PlayerData playerData;

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
