using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Placeable Object", menuName = "Inventory System/Items/Equipment")]
public class SO_Equipment : SO_Item
{
    public void Awake()
    { 
        type = ItemType.Equipment;
        sellable = false;
        isPlaceable = false;
    }
}
