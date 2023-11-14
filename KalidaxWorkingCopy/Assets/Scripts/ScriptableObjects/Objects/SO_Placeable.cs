using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Placeable Object", menuName = "Inventory System/Items/Placeable")]
public class SO_Placeable : SO_Item
{
    public void Awake()
    {
        type = ItemType.Placeable;
        isPlaceable = true;
    }
}
