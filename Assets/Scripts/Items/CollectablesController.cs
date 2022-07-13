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

        UpdateCompleteLevelRecordScreen();
        UpdateCompleteLevelCurrentScreen();
    }

    public void Collect(string name)
    {
        fruitsCollected[Consts.GetFruitIndex(name)]++;
             
        UpdateCompleteLevelCurrentScreen();
    }

    private void UpdateCompleteLevelRecordScreen()
    {
        foreach (Transform child in CompleteLevelRecord)
        {
            int fruitIndex = Consts.GetFruitIndex(child.GetComponent<Image>().name);
            if (fruitsCount[fruitIndex] > 0)
            {
                child.GetComponentInChildren<Text>().text
                    = GameManager.fruitRecords[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)][fruitIndex] + "/" + fruitsCount[fruitIndex];
            }
        }    
    }

    private void UpdateCompleteLevelCurrentScreen()
    {
        foreach (Transform child in CompleteLevelCurrent)
        {
            int fruitIndex = Consts.GetFruitIndex(child.GetComponent<Image>().name);
            if (fruitsCount[fruitIndex] > 0)
            {
                child.GetComponentInChildren<Text>().text = fruitsCollected[fruitIndex] + "/" + fruitsCount[fruitIndex];
            }
        }
    }
}
