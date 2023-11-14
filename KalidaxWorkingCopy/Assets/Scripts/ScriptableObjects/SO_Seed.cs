using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_Seed : SO_Item
{
    public void Awake()
    {
        type = ItemType.Seed;
        isPlaceable = false;
    }
}
