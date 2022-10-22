using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public int currentItem;
    public List<Items> items;
    public List<Sprite> ItemIcons;
    public Image currentItemIcon;

    public Sprite darkBladeIcon;
    public Sprite goldenAxeIcon;

    void Start()
    {
        currentItem = 0;
        UpdateEquipment();
    }

    public void UpdateEquipment()
    {
        items = Enumerable.Repeat(Items.Empty, 6).ToList();

        items[0] = Items.Swoosh;

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL1_3)] == true)
        {
            items[1] = Items.Axe;
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL2_3)] == true)
        {
            items[2] = Items.Stone;
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_1)] == true)
        {
            items[3] = Items.FireSpark;
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_2)] == true)
        {
            items[4] = Items.ArcticBreathe;
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_3)] == true)
        {
            ItemIcons[0] = darkBladeIcon;
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_4)] == true)
        {
            items[5] = Items.Poison;
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL4_1)] == true)
        {
            ItemIcons[1] = goldenAxeIcon;
        }

        currentItemIcon.sprite = ItemIcons[currentItem];
    }

    public void ChangeItem()
    {
        do
        {
            currentItem++;
            if (currentItem == items.Count)
            {
                currentItem = 0;
            }
        } while (items[currentItem] == Items.Empty);

        currentItemIcon.sprite = ItemIcons[currentItem];
    }

    public enum Items { Empty = -1, Swoosh, Axe, Stone, FireSpark, ArcticBreathe, Poison };
}
