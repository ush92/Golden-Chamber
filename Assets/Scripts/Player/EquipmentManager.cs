using System.Collections.Generic;
using System.Reflection;
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
        items = new List<Items>();
        UpdateEquipment();
    }

    public void UpdateEquipment()
    {
        if (!items.Contains(Items.Swoosh))
        {
            items.Add(Items.Swoosh);
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL1_3)] == true)
        {
            if(!items.Contains(Items.Axe))
            {
                items.Add(Items.Axe);
            }        
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL2_3)] == true)
        {
            if (!items.Contains(Items.Stone))
            {
                items.Add(Items.Stone);
            }
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_1)] == true)
        {
            if (!items.Contains(Items.FireSpark))
            {
                items.Add(Items.FireSpark);
            }
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_2)] == true)
        {
            if (!items.Contains(Items.ArcticBreathe))
            {
                items.Add(Items.ArcticBreathe);
            }
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_3)] == true)
        {
            ItemIcons[0] = darkBladeIcon;          
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_4)] == true)
        {
            if (!items.Contains(Items.Poison))
            {
                items.Add(Items.Poison);
            }
        }

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL4_1)] == true)
        {
            ItemIcons[1] = goldenAxeIcon;
        }

        currentItemIcon.sprite = ItemIcons[currentItem];
    }

    public void ChangeItem()
    {
        currentItem++;

        if (currentItem == items.Count)
        {
            currentItem = 0;
        }

        currentItemIcon.sprite = ItemIcons[currentItem];
    }

    public enum Items { Swoosh, Axe, Stone, FireSpark, ArcticBreathe, Poison };
}
