using UnityEngine;
using UnityEngine.UI;

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

    public Transform CompleteLevelRecord;
    public Transform CompleteLevelCurrent;

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
        foreach (Transform child in CompleteLevelRecord)
        {
            switch (child.GetComponentInChildren<Image>().name)
            {
                case Consts.APPLE:
                    child.GetComponentInChildren<Text>().text = "99" + "/" + applesCount;
                    break;
                case Consts.BANANA:
                    child.GetComponentInChildren<Text>().text = "99" + "/" + bananasCount;
                    break;
                case Consts.STRAWBERRY:
                    child.GetComponentInChildren<Text>().text = "99" + "/" + strawberriesCount;
                    break;
                case Consts.CHERRY:
                    child.GetComponentInChildren<Text>().text = "99" + "/" + cherriesCount;
                    break;
                case Consts.ORANGE:
                    child.GetComponentInChildren<Text>().text = "99" + "/" + orangesCount;
                    break;
                case Consts.KIWI:
                    child.GetComponentInChildren<Text>().text = "99" + "/" + kiwisCount;
                    break;
                case Consts.MELON:
                    child.GetComponentInChildren<Text>().text = "99" + "/" + melonsCount;
                    break;
                case Consts.ANANAS:
                    child.GetComponentInChildren<Text>().text = "99" + "/" + ananasCount;
                    break;
                case Consts.COIN:
                    child.GetComponentInChildren<Text>().text = "99" + "/" + coinsCount;
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateCompleteLevelCurrentScreen()
    {
        foreach (Transform child in CompleteLevelCurrent)
        {
            switch (child.GetComponentInChildren<Image>().name)
            {
                case Consts.APPLE:
                    child.GetComponentInChildren<Text>().text = applesCollected + "/" + applesCount;
                    break;
                case Consts.BANANA:
                    child.GetComponentInChildren<Text>().text = bananasCollected + "/" + bananasCount;
                    break;
                case Consts.STRAWBERRY:
                    child.GetComponentInChildren<Text>().text = strawberriesCollected + "/" + strawberriesCount;
                    break;
                case Consts.CHERRY:
                    child.GetComponentInChildren<Text>().text = cherriesCollected + "/" + cherriesCount;
                    break;
                case Consts.ORANGE:
                    child.GetComponentInChildren<Text>().text = orangesCollected + "/" + orangesCount;
                    break;
                case Consts.KIWI:
                    child.GetComponentInChildren<Text>().text = kiwisCollected + "/" + kiwisCount;
                    break;
                case Consts.MELON:
                    child.GetComponentInChildren<Text>().text = melonsCollected + "/" + melonsCount;
                    break;
                case Consts.ANANAS:
                    child.GetComponentInChildren<Text>().text = ananasCollected + "/" + ananasCount;
                    break;
                case Consts.COIN:
                    child.GetComponentInChildren<Text>().text = coinsCollected + "/" + coinsCount;
                    break;
                default:
                    break;
            }
        }
    }
}
