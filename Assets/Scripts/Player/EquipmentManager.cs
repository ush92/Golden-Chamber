using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public List<Items> items;

    public List<Sprite> ItemIcons;
    public Image currentItemIcon;

    public int currentItem;

    void Start()
    {
        currentItem = 0;
        items = new List<Items>();
    }

    public void UpdateEquipment()
    {
        currentItemIcon.sprite = ItemIcons[currentItem];

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

        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_4)] == true)
        {
            if (!items.Contains(Items.Poison))
            {
                items.Add(Items.Poison);
            }
        }
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
