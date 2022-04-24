using System;
using System.IO;
using UnityEngine;

public static class FileManager
{
    public static bool WriteToFile(string fileName, string fileContents)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.WriteAllText(fullPath, fileContents);
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log($"failed to write to {fullPath} with exception {ex}");
        }

        return false;
    }

    public static bool LoadFromFile(string fileName, out string result)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"failed to read from {fullPath} with exception {ex}");
            result = "";            
            return false;
        }    
    }
}
