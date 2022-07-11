using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class CollectablesController : MonoBehaviour
{
    public int cherriesCount;
    public int cherriesCollected;
    public int ananasCount;
    public int ananasCollected;
    public int kiwisCount;
    public int kiwisCollected;
    public int applesCount;
    public int applesCollected;
    public int orangesCount;
    public int orangesCollected;
    public int melonsCount;
    public int melonsCollected;
    public int strawberriesCount;
    public int strawberriesCollected;
    public int bananasCount;
    public int bananasCollected;
    public int coinsCount;
    public int coinsCollected;

    public GameObject CompleteLevelRecord;
    public GameObject CompleteLevelCurrent;

    void Start()
    {
        foreach (Transform child in transform)
        {
            switch(child.GetComponent<Collectable>().name)
            {
                case Consts.APPLE:
                    applesCount++;
                    break;
                case Consts.BANANA:
                    bananasCount++;
                    break;
                case Consts.STRAWBERRY:
                    strawberriesCount++;
                    break;
                case Consts.CHERRY:
                    cherriesCount++;
                    break;
                case Consts.ORANGE:
                    orangesCount++;
                    break;
                case Consts.KIWI:
                    kiwisCount++;
                    break;
                case Consts.MELON:
                    melonsCount++;
                    break;
                case Consts.ANANAS:
                    ananasCount++;
                    break;
                case Consts.COIN:
                    coinsCount++;
                    break;
                default:
                    break;
            }
        }

        UpdateCompleteLevelRecordScreen();
        UpdateCompleteLevelCurrentScreen();
    }

    public void Collect(string name)
    {
        switch (name)
        {
            case Consts.APPLE:
                applesCollected++;
                break;
            case Consts.BANANA:
                bananasCollected++;
                break;
            case Consts.STRAWBERRY:
                strawberriesCollected++;
                break;
            case Consts.CHERRY:
                cherriesCollected++;
                break;
            case Consts.ORANGE:
                orangesCollected++;
                break;
            case Consts.KIWI:
                kiwisCollected++;
                break;
            case Consts.MELON:
                melonsCollected++;
                break;
            case Consts.ANANAS:
                ananasCollected++;
                break;
            case Consts.COIN:
                coinsCollected++;
                break;
            default:
                break;
        }
        
        UpdateCompleteLevelCurrentScreen();
    }

    private void UpdateCompleteLevelRecordScreen()
    {

    }

    private void UpdateCompleteLevelCurrentScreen()
    {

    }
}
