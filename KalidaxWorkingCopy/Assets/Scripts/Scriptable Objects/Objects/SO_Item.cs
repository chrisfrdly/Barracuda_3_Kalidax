using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keeping track of all the item types in the game 
public enum ItemType
{
    Seed,
    Equipment,
    Placeable,
    Default
}
//this section is to establish the framework for equippable/consumable items
public enum Attributes
{
    Speed,
    Research,
    Farming,
    Selling
}

//holding our data for other stuff
public class SO_Item : ScriptableObject
{
    public int Id;

    //these are the attributes of our objects
    public Sprite uiDisplay;
    public int sellValue;
    public bool sellable;
    public bool isPlaceable;
    public ItemType type;

    //this section is if we want to buff our players
    public ItemBuff[] buffs;
    //this is gonna include the description of our objects
    [TextArea(15,20)]
    public string description;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public int id;
    //this section is if we want to buff our players
    public ItemBuff[] buffs;
    public Item(SO_Item item)
    {
        name = item.name;
        id = item.Id;
        buffs = new ItemBuff[item.buffs.Length];    

        for(int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max);
            buffs[i].attributes = item.buffs[i].attributes;
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    //this is framework for items that may have random buffs
    public Attributes attributes;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
    }
    public void GenerateValue()
    {
        //creates a random value for whatever attribute this seeks to maximize
        value = UnityEngine.Random.Range(min, max);
    }
}
