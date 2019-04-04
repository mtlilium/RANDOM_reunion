using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorageScript : MonoBehaviour
{
    Dictionary<int, int> Inventory= new Dictionary<int, int>();

    public void AddItem(string itemname, int quantity)
    {
        int itemid =ItemInfo.ItemIDResolution;
        if (Inventory.ContainsKey(itemid))
        {
            Inventory[itemid] += quantity;
        }
        else
        {
            Inventory.Add(itemid, 1);
        }
    }
    public void AddItem(int itemid, int quantity)
    {
        if (Inventory.ContainsKey(itemid))
        {
        }
        else
        {
            Inventory.Add(itemid, 1);
        }
    }
    public bool RemoveItem(string itemname, int quantity)
    {
        int itemid = ItemInfo.ItemIDResolution;
        if (Inventory.ContainsKey(itemid))
        {
            if (Inventory[itemid] >= quantity)
            {
                Inventory[itemid] -= quantity;
                return true;
            }

        }
        return false;
    }
    public bool RemoveItem(int itemid, int quantity)
    {
        if (Inventory.ContainsKey(itemid))
        {
            if (Inventory[itemid] >= quantity)
            {
                Inventory[itemid] -= quantity;
                return true;
            }
            
        }
        return false;
    }
}