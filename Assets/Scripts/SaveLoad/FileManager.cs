using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public static bool IsFileExist(string fileName)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            if(File.Exists(fullPath))
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"failed to check path {fullPath} with exception {ex}");
        }
        return false;
    }

    public static List<string> GetAllProfiles()
    {
        var files = new DirectoryInfo(Path.Combine(Application.persistentDataPath))
                        .GetFiles("*.dat")
                        .OrderByDescending(f => f.LastWriteTime)
                        .ToArray();

        var result = new List<string>();
        try
        {
            foreach(var file in files)
            {              
                result.Add(Path.GetFileNameWithoutExtension(file.FullName));              
            }
            return result;
        }
        catch (Exception ex)
        {
            Debug.Log($"failed to check path {Path.Combine(Application.persistentDataPath)} with exception {ex}");
        }
        return null;
    }
}
