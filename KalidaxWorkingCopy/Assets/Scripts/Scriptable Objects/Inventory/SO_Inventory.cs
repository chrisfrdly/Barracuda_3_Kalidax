using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class SO_Inventory : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    public SO_ItemDatabase database;
    public Inventory container;


    //this will check our list to see if we have an item in our inventory, if not, we can add one
    public void AddItem(Item _item, int _amount)
    {
        //this is the buffs section again
        //additionally this will help for non-stackable items. They don't need to give a buff, but if it has the attribute, it will not stack
        if(_item.buffs.Length > 0)
        {
            container.items.Add(new InventorySlot(_item.id, _item, _amount));
            return;
        }

        for (int i = 0; i < container.items.Count; i++)
        {
            //checks if the item is already in our inventory 
            if (container.items[i].item.id == _item.id)
            {
                container.items[i].AddAmount(_amount);
                return;
            }
        }
        Debug.Log(container.items.Count);
        container.items.Add(new InventorySlot(_item.id, _item, _amount));
    }

    //turns our scriptable object into a string and then will convert it into a .Json file
    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    //this loads our file back onto our scene and converts it back into a scriptable object with the data filled in
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Inventory();
    }


    //for Unity to serialize our inventory whenever we change it in editor. Will always match the item id
    public void OnAfterDeserialize()
    {
        for(int i = 0; i < container.items.Count; i++)
        {
            container.items[i].item = container.items[i].item;
            //database.getItem[container.items[i].id]
        }
    }
    public void OnBeforeSerialize()
    {

    }
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> items = new List<InventorySlot>();
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
    public Item item;
    public int amount;
    
    public InventorySlot(int _id, Item _item, int _amount)
    {
        id = _id;
        item = _item;
        amount = _amount;
    }

    //this is gonna add a quanity of an item to our inventory slots
    public void AddAmount(int value)
    {
        amount += value;
    }
}
