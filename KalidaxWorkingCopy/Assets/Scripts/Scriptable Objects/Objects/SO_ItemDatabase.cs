using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName ="Inventory System/Items/Database")]
public class SO_ItemDatabase : ScriptableObject, ISerializationCallbackReceiver 
{
    public SO_Item[] items;
    public Dictionary<SO_Item, int> getID = new Dictionary<SO_Item, int>();
    public Dictionary<int,SO_Item> getItem = new Dictionary<int, SO_Item>();

    public void OnAfterDeserialize()
    {
        //clears our dictionary so there are no duplicates
        getID = new Dictionary<SO_Item, int>();
        getItem = new Dictionary<int, SO_Item>();
        //Whenever this object is serialized, it will add the items in the editor
        for(int i = 0; i < items.Length; i++)
        {
            getID.Add(items[i], i);
            getItem.Add(i, items[i]);
        }
    }

    //I need this to serialize the dictionary but there will be nothing here
    public void OnBeforeSerialize()
    {

    }
}
