using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public SO_Inventory inventory;

    public int startPosition_x;
    public int startPosition_y;

    //these variables are to edit how big the inventory actually is
    public int numberOfColumn;
    public int spaceBetweenItem_x;
    public int spaceBetweenItem_y;

    //Dictionary that has all the items within the Inventory Slot
    private Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject> ();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        //this creates our little inventory menu based on how many objects we are holding
        for(int i = 0; i < inventory.container.Count; i++)
        {
            //creates the object using the item prefab
            var obj = Instantiate(inventory.container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);

            //this spaces the stuff in the inventory around and also shows the number
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");
            itemsDisplayed.Add(inventory.container[i], obj); //saves this object into the dictionary
        }
    }

    //this method updates the values of your items and adds new items to your inventory when you pick them up
    public void UpdateDisplay()
    {
       for(int i = 0; i < inventory.container.Count;i++)
        {
            ///
            /// this checks if your item is in your inventory already
            /// if it is, it displays the value in whatever slot your item is in
            /// if it is not, it will create a new space for you to hold your item, assuming the inventory is not full
            ///
            if(itemsDisplayed.ContainsKey(inventory.container[i]))
            {
                itemsDisplayed[inventory.container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");
            }
            else
            {
                //creates the object using the item prefab
                var obj = Instantiate(inventory.container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);

                //this spaces the stuff in the inventory around and also shows the number
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.container[i], obj); //saves this object into the dictionary
            }
        }
    }

    //this function returns positions for our items as a grid
    public Vector3 GetPosition(int i)
    {
        return new Vector3(startPosition_x + (spaceBetweenItem_x * (i % numberOfColumn)),startPosition_y + (-spaceBetweenItem_y * (i / numberOfColumn)), 0f);
    }
}
