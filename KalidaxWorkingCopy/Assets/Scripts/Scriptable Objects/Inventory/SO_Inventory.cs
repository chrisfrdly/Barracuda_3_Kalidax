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
        if (_item.buffs.Length > 0)
        {
            SetEmptySlot(_item, _amount);
            return;
        }

        for (int i = 0; i < container.items.Length; i++)
        {
            //checks if the item is already in our inventory 
            if (container.items[i].id == _item.id)
            {
                container.items[i].AddAmount(_amount);
                return;
            }
        }
        SetEmptySlot(_item, _amount);
 
    }

    //finds and returns the first empty inventory slot
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if(container.items[i].id <= -1)
            {
                container.items[i].UpdateSlot(_item.id, _item, _amount);
                return container.items[i];
            }
        }
        //set up what happens when inventory is full
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.id, item2.item, item2.amount);
        item2.UpdateSlot(item1.id, item1.item, item1.amount);
        item1.UpdateSlot(temp.id, temp.item, temp.amount);
    }

    //this will take an item out of the inventory
    public void RemoveItem(Item _item)
    {
        for(int i = 0; i < container.items.Length; i++)
        {
            if(container.items[i].item == _item)
            {
                container.items[i].UpdateSlot(-1, null, 0);
            }
        }
    }

    public void SellItem(Item _item, int amount)
    {
        if(_item.sellable)
        {
            PlayerWallet.instance.amountToPutInWallet += _item.sellValue * amount;
            RemoveItem(_item);
        }
        else
        {
            Debug.Log("This Item is not Sellable");
            return;
        }
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
        for(int i = 0; i < container.items.Length; i++)
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
    public InventorySlot[] items = new InventorySlot[24];
}
/// <summary>
/// Each of these classes are gonna be a container for our in game items
/// it will be a slot that contains all the data of the required items
/// this is not its own script because the inventory is the only thing that uses this 
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public int id = -1;
    public Item item;
    public int amount;

    //default constructor 
    public InventorySlot()
    {
        id = -1;
        item = null;
        amount = 0;
    }
    public InventorySlot(int _id, Item _item, int _amount)
    {
        id = _id;
        item = _item;
        amount = _amount;
    }

    public void UpdateSlot(int _id, Item _item, int _amount)
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
