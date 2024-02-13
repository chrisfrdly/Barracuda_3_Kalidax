using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporarySellingScript : MonoBehaviour
{
    public SO_Inventory inventoryScript;
    public WorldAlien alienToSell;
    public int amount;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            inventoryScript.SellItem(FindFirstSellableItem(), amount);
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            alienToSell.isBeingSold = true;
        }
    }

    public Item FindFirstSellableItem()
    {
        for(int i = 0; i < inventoryScript.container.items.Length; i++)
        {
            if (inventoryScript.container.items[i].id > -1)
            {
                amount = inventoryScript.container.items[i].amount;
                return inventoryScript.container.items[i].item;
            }
        }
        return null;
        
    }
}
