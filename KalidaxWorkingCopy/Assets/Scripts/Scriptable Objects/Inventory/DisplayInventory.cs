using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    /// <summary>
    /// this script is just a visual representation of what is happening in the backend with our SO_Inventory Script
    /// this is what updates the UI display for the player
    /// it is not responsible for any inventory calculation, all of that can be found in SO_Inventory
    /// </summary>
    
    public GameObject inventoryPrefab;

    public SO_Inventory inventory;

    // this is where the item will be displayed on the UI
    public int startPosition_x;
    public int startPosition_y;

    //these variables are to edit how big the inventory actually is
    public int numberOfColumn;
    public int spaceBetweenItem_x;
    public int spaceBetweenItem_y;

    //Dictionary that has all the items within the Inventory Slot
    private Dictionary<GameObject , InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }

    //this will spawn in all of the slots without objects in them
    public void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.container.items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            itemsDisplayed.Add(obj, inventory.container.items[i]);
        }
    }

    public void UpdateSlots()
    {
        //loops through our dictionary to find any changes within the obejct and updates the graphic to the appropriate sprite
        foreach(KeyValuePair<GameObject , InventorySlot> _slot in itemsDisplayed)
        {
            //this checks if there is an item within our slot
            if(_slot.Value.id >= 0)
            {
                //if this looks weird it's because of the way that we save our inventory when the application is closed
                //we don't need to care about saving the sprite because those are updated at runtime
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.getItem[_slot.Value.id].uiDisplay;
                //setting the color ourselves cuz I there's no need to render the slot if the item is gonna be in it 
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                //if we only have one of the item in our inventory slot, it will display no text, else it will display the amount
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                //Making this transparent cuz we don't need to render it rn
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                //if we only have one of the item in our inventory slot, it will display no text, else it will display the amount
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }


    //this function returns positions for our items as a grid
    public Vector3 GetPosition(int i)
    {
        return new Vector3(startPosition_x + (spaceBetweenItem_x * (i % numberOfColumn)), startPosition_y + (-spaceBetweenItem_y * (i / numberOfColumn)), 0f);
    }
}
