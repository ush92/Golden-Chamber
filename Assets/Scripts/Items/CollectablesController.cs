using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectablesController : MonoBehaviour
{
    public List<int> fruitsCount = new List<int>(new int[9]);
    public List<int> fruitsCollected = new List<int>(new int[9]);

    public Transform CompleteLevelRecord;
    public Transform CompleteLevelCurrent;

    void Start()
    {
        foreach (Transform child in transform)
        {
            fruitsCount[Consts.GetFruitIndex(child.GetComponent<Collectable>().name)]++;
        }



        LoadCompleteLevelRecord();
        UpdateCompleteLevelCurrent();
    }

    public void Collect(string name)
    {
        fruitsCollected[Consts.GetFruitIndex(name)]++;
             
        UpdateCompleteLevelCurrent();
    }

    private void LoadCompleteLevelRecord()
    {
        foreach (Transform child in CompleteLevelRecord)
        {
            int fruitIndex = Consts.GetFruitIndex(child.GetComponent<Image>().name);
            if (fruitsCount[fruitIndex] > 0)
            {
                child.GetComponentInChildren<Text>().text
                    = GameManager.levelRecords[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)][fruitIndex] + "/" + fruitsCount[fruitIndex];
            }
        }    
    }

    private void UpdateCompleteLevelCurrent()
    {
        foreach (Transform child in CompleteLevelCurrent)
        {
            int fruitIndex = Consts.GetFruitIndex(child.GetComponent<Image>().name);
            if (fruitsCount[fruitIndex] > 0)
            {
                child.GetComponentInChildren<Text>().text = fruitsCollected[fruitIndex] + "/" + fruitsCount[fruitIndex];

                int record = GameManager.levelRecords[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)][fruitIndex];
                if (fruitsCollected[fruitIndex] < record)
                {
                    child.GetComponentInChildren<Text>().color = new Color32(220, 20, 60, 255);
                }
                else if(fruitsCollected[fruitIndex] == record)
                {
                    child.GetComponentInChildren<Text>().color = new Color32(182, 182, 182, 255);
                }
                else
                {
                    child.GetComponentInChildren<Text>().color = new Color32(60, 179, 113, 255);
                }
            }
        }
    }

    public void UpdateLevelFruitRecord(int levelIndex)
    {
        foreach (Transform child in CompleteLevelCurrent)
        {
            int fruitIndex = Consts.GetFruitIndex(child.GetComponent<Image>().name);
            if (fruitsCollected[fruitIndex] > GameManager.levelRecords[levelIndex][fruitIndex])
            {
                GameManager.levelRecords[levelIndex][fruitIndex] = fruitsCollected[fruitIndex];
            }
        }
    }
}
