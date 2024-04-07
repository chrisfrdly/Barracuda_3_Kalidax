using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractableObject_Provisions_Drone : InteractableObject
{
    //necessary UI stuff
    [Header("Panel")]
    [SerializeField] private GameObject pDroneHUDPanel;

    [Header("Button")]
    [SerializeField] private Button sellButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Other Contents")]
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject needSeedText;



    [SerializeField] private SO_Inventory inventory; //this is so you can sell your seeds
    [SerializeField] private int sellAmount; //this is how many seeds you will sell


    // Start is called before the first frame update
    void Start()
    {
        base.Awake();
        SO_interactableObject.clickedCancelButtonEvent.AddListener(CloseInteractionPrompt);
    }


    //this will run when the player interacts with this object
    public override void OnInteract(GameObject _interactedActor)
    {
        OpenInteractionPanel();

        AudioManager.instance.Play("Positive Interact");

    }

    //This is temporary but it will make you sell all your seeds we can UX the shit out of this so the player knows they're selling all of them
    //eventually we want the player to select how many seeds they sell but we can backlog all of this for now 
    public void SellAllSeeds()
    {
        bool soldSomething = false;
        for (int i = 0; i < inventory.container.items.Length; i++)
        {
            if (inventory.container.items[i].id > -1)
            {
                sellAmount += inventory.container.items[i].amount;
                inventory.SellItem(inventory.container.items[i].item, sellAmount);
                sellAmount = 0;
                soldSomething = true;
                ClearItemImage();
            }
            else if (i == 0 && inventory.container.items[i].id < 1)
            {
                //They don't have any seeds
                Instantiate(needSeedText, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity, pDroneHUDPanel.transform);
                soldSomething = false;
            }
        }

        //now play the corresponding audio if we sold an item or not
        if(soldSomething)
        {
            AudioManager.instance.Play("Item Sold Coins");
        }
        else
        {
            AudioManager.instance.Play("Negative Interact");
        }
        
    }


    //------------------
    //Open and Close HUD
    //------------------

    private void OpenInteractionPanel()
    {
        if (PlayerInputHandler.Instance.GetCurrentControlScheme() == "Controller")
        {
            PlayerInputHandler.Instance.SwitchActionMap(true);
            sellButton.Select();
        }

        //make the image based on what they have in their inventory
        if (inventory.container.items[0].amount > 0)
        {
            itemImage.color = new Color(1, 1, 1, 1); //revealing the alpha

            //Get the Id of the firs slot item
            int itemID = inventory.container.items[0].id;
            itemImage.sprite = inventory.database.getItem[itemID].uiDisplay;
        }
        else
        {
            ClearItemImage();
        }

        //Activate the panel and make it the currentVisible UI
        pDroneHUDPanel.SetActive(true);
        UIController.Instance.m_CurrentUIVisible = pDroneHUDPanel;
    }

    private void ClearItemImage()
    {
        itemImage.sprite = null;
        itemImage.color = new Color(1, 1, 1, 0); //hiding the alpha
    }
    private void CloseInteractionPrompt()
    {
        pDroneHUDPanel.SetActive(false);

    }



    public override bool CheckIsInteractable() { return isInteractable; }
    public override bool IsTargetPointVisible() { return isInteractPointVisible; }
    public override bool FreezePlayerMovement() { return freezePlayerMovement; }
    public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }
}
