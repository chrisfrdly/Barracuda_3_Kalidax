using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public SO_Inventory inventory;

    //will pick up any item that is on the ground and add it to the inventory 
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<Item>();
        //checks if this item does not return null
        if(item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(collision.gameObject);
        }
    }

    //this resets the inventory when you stop playing the game
    private void OnApplicationQuit()
    {
        inventory.container.Clear();
    }
}
