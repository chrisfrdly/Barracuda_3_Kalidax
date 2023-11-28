using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class SO_Inventory : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    public SO_ItemDatabase database;
    public List<InventorySlot> container = new List<InventorySlot> ();

    //this will check our list to see if we have an item in our inventory, if not, we can add one
    public void AddItem(SO_Item _item, int _amount)
    {
        for (int i = 0; i < container.Count; i++)
        {
            //checks if the item is already in our inventory 
            if (container[i].item == _item)
            {
                container[i].AddAmount(_amount);
                return;
            }
        }
        
        container.Add(new InventorySlot(database.getID[_item], _item, _amount));
    }

    //turns our scriptable object into a string and then will convert it into a .Json file

    //for Unity to serialize our inventory whenever we change it in editor. Will always match the item id
    public void OnAfterDeserialize()
    {
        for(int i = 0; i < container.Count; i++)
        {
            container[i].item = database.getItem[container[i].id];
        }
    }
    public void OnBeforeSerialize()
    {

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
    public int id;
    public SO_Item item;
    public int amount;
    
    public InventorySlot(int _id, SO_Item _item, int _amount)
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
