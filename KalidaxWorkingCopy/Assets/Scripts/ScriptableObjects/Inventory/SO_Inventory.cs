using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class SO_Inventory : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot> ();

    //this will check our list to see if we have an item in our inventory, if not, we can add one
    public void AddItem(SO_Item _item, int _amount)
    {
        bool hasItem = false;
        for(int i = 0; i < container.Count; i++)
        {
            //checks if the item is already in our inventory 
            if(container[i].item == _item)
            {
                container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }

        if(!hasItem)
        {
            container.Add(new InventorySlot(_item, _amount));
        }
    }
}
/// <summary>
/// Each of these classes are gonna be a container for our in game items
/// it will be a slot that contains all the data of the required items
/// this is not its own script because the inventory is the only thing that uses this 
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public SO_Item item;
    public int amount;
    
    public InventorySlot(SO_Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    //this is gonna add a quanity of an item to our inventory slots
    public void AddAmount(int value)
    {
        amount += value;
    }
}
