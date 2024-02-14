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

    public SO_Inventory inventory; //this is so you can sell your seeds
    [SerializeField] private int sellAmount; //this is how many seeds you will sell


    // Start is called before the first frame update
    void Start()
    {
        base.Awake();
        SO_interactableObject.clickedCancelButtonEvent.AddListener(CloseInteractionPrompt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //this will run when the player interacts with this object
    protected override void OnInteract()
    {
        OpenInteractionPanel();
        HideUI();
    }

    //This is temporary but it will make you sell all your seeds we can UX the shit out of this so the player knows they're selling all of them
    //eventually we want the player to select how many seeds they sell but we can backlog all of this for now 
    public void SellAllSeeds()
    {
        for (int i = 0; i < inventory.container.items.Length; i++)
        {
            if (inventory.container.items[i].id > -1)
            {
                sellAmount += inventory.container.items[i].amount;
                inventory.SellItem(inventory.container.items[i].item, sellAmount);
                sellAmount = 0;
            }
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

        //Activate the panel and make it the currentVisible UI
        pDroneHUDPanel.SetActive(true);
        UIController.Instance.m_CurrentUIVisible = pDroneHUDPanel;
    }

    private void CloseInteractionPrompt()
    {
        pDroneHUDPanel.SetActive(false);

    }



    protected override bool IsInteractable() { return isInteractable; }
    protected override bool IsTargetPointVisible() { return isInteractPointVisible; }
    protected override bool FreezePlayerMovement() { return freezePlayerMovement; }
    public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }
}
