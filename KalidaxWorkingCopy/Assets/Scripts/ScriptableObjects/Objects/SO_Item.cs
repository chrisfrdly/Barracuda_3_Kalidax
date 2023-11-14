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

//holding our data for other stuff
public class SO_Item : ScriptableObject
{
    public GameObject prefab;
    public int sellValue;
    public bool sellable;
    public bool isPlaceable;
    public ItemType type;

    //this is gonna include the description of our objects
    [TextArea(15,20)]
    public string description;
}
