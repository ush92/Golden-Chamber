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
    }

    public void ChangeItem()
    {
        if(currentItem == items.Count - 1)
        {
            currentItem = 0;
        }
        else
        {
            currentItem++;
        }

        currentItemIcon.sprite = ItemIcons[currentItem];
    }

    public enum Items { Swoosh, Axe };
}
