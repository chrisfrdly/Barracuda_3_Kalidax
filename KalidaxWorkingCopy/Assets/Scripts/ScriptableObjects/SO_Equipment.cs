using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_Equipment : SO_Item
{
    public void Awake()
    {
        type = ItemType.Placeable;
        isPlaceable = false;
    }
}
