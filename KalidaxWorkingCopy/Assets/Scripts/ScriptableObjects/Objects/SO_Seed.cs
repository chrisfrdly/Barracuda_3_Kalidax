using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Placeable Object", menuName = "Inventory System/Items/Seed")]
public class SO_Seed : SO_Item
{
    public void Awake()
    {
        type = ItemType.Seed;
        isPlaceable = false;
    }
}
