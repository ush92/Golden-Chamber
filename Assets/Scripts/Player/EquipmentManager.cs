using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public List<Items> items;
    public int currentItem;
    public Image currentItemIcon;

    void Start()
    {
        items = new List<Items>() { Items.Swoosh };
        UpdateEquipment();
    }

    public void UpdateEquipment()
    {
        if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL1_3)] == true)
        {
            if(!items.Contains(Items.Axe))
            {
                items.Add(Items.Axe);
            }        
        }
    }

    public enum Items { Swoosh, Axe };
}
