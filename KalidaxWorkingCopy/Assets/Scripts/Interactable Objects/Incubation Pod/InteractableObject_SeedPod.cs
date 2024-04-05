using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public enum IncubationState
{
    OBJ_AddSeed,
    OBJ_Incubating,
    OBJ_RemoveSeed,
    OBJ_NotPurchased
}


public class InteractableObject_SeedPod : InteractableObject
{

    private IncubationState incubationState = IncubationState.OBJ_NotPurchased;

    [Separator()]
    [SerializeField] private SO_GameEvent gameEvent;
    public bool incubationCompleteTriggered = false;
    [Separator()]
    [SerializeField] private SO_Data_DayCycle dataDayCycle;

    [Header("Panel")]
    [SerializeField] private GameObject incubationPodHUDPanel;

    [Header("Button")]
    [SerializeField] private Button addSeedButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject needSeedPrefab;

    [Header("Other Contents")]
    [SerializeField] private TextMeshProUGUI daysRemainingText;
    [SerializeField] private Image seedImage;
    [SerializeField] private SpriteRenderer incubationLight;
    [SerializeField] private Vector3 spawnLocation;
    public SO_Inventory inventory;

    [Header("Incubation Parameters")]
    [SerializeField]private int daysToIncubate = 2;
    private int daysLeft;
    private int seedIndex;

    [Header("Prefab Instantiation")]
    [SerializeField] private List<GameObject> T1alienPrefabs;

    [SerializeField] private Color incubationColour_AddSeed;
    [SerializeField] private Color incubationColour_Incubating;
    [SerializeField] private Color incubationColour_RemoveSeed;
    [SerializeField] private Color incubationColour_NotPurchased;
    //serialized array

    private int thisIndex;

    //Properties
    public int m_DaysLeft { get => daysLeft; set => daysLeft = value; }
    public int m_ThisIndex { get => thisIndex; set => thisIndex = value; }
    public IncubationState m_IncubationState { get => incubationState; set => incubationState = value; }

    /// <summary>
    /// The way the incubation works is that at the beginning you must add a seed into it,
    /// Then it starts the incubation process. You cannot take out the seed from it
    /// Then when it's done incubating (remaining days = 0), the player can remove the seed. 
    /// Once removed we go back to the add seed state
    /// </summary>

    protected override void Awake()
    {
        base.Awake();
        SO_interactableObject.clickedCancelButtonEvent.AddListener(CloseInteractionPrompt);
        daysLeft = daysToIncubate;

    }
    private void Start()
    {

        UpdateData();
    }
    private void UpdateData()
    {
        //Now update the pod with the new Day's Data

        if (dataDayCycle.incubationPodData[thisIndex].index == -1)
        {
            //add this object to that list if it's not already in the list
            dataDayCycle.incubationPodData[thisIndex] = new IncubationPodData();
            dataDayCycle.incubationPodData[thisIndex].index = thisIndex;

            //set the first incubation pod to already purchased
            bool purchased = dataDayCycle.incubationPodPurchased[thisIndex];

            if (purchased)
            {
                AudioManager.instance.Play("Item Purchased Coins");

                dataDayCycle.incubationPodData[thisIndex].incubationState = IncubationState.OBJ_AddSeed;
                incubationState = IncubationState.OBJ_AddSeed;
            }


            dataDayCycle.incubationPodData[thisIndex].incubationState = incubationState;
            dataDayCycle.incubationPodData[thisIndex].daysLeft = daysLeft;
        }

        //If we already have this object's data stored, retrieve the data
        else
        {

            incubationState = dataDayCycle.incubationPodData[thisIndex].incubationState;


            if (incubationState == IncubationState.OBJ_Incubating)
            {
                daysLeft = dataDayCycle.incubationPodData[thisIndex].daysLeft - 1;
                dataDayCycle.incubationPodData[thisIndex].daysLeft = daysLeft;
                seedIndex = dataDayCycle.incubationPodData[thisIndex].seedIndex;
            }

            // Check if still incubating
            if (incubationState == IncubationState.OBJ_Incubating && daysLeft > 0)
            {
                // If still incubating, raise the "Seed Placed" event
                gameEvent.RaiseProgressChanged(ProgressState.Incubating);
            }

            //Check to see if the days left <= 1 and if so, se the incubation state to Complete
            if (daysLeft <= 0)
            {
                incubationState = IncubationState.OBJ_RemoveSeed;
                dataDayCycle.incubationPodData[thisIndex].incubationState = incubationState;
                gameEvent.RaiseProgressChanged(ProgressState.IncubationComplete);
                incubationCompleteTriggered = true;
            }


            Debug.Log(seedIndex);
            //now we check to see if this incubation pod has been purchased and if so, then set the state to green
        }


        SetColourOfPodLight();
    }

    //called in Awake and also in the "UpgradeCheck.cs" script
    public void SetColourOfPodLight()
    {

        //set the colour of the light to the incubation state
        switch (incubationState)
        {
            case IncubationState.OBJ_AddSeed:
                incubationLight.color = incubationColour_AddSeed;
                break;

            case IncubationState.OBJ_Incubating:
                incubationLight.color = incubationColour_Incubating;

                break;

            case IncubationState.OBJ_RemoveSeed:
                incubationLight.color = incubationColour_RemoveSeed;

                break;
            case IncubationState.OBJ_NotPurchased:
                incubationLight.color = incubationColour_NotPurchased;
                break;

        }
    }
    public override void OnInteract(GameObject _interactedActo)
    {
        OpenInteractionPanel();

        HideUI();

    }

    public void SetInteractable(bool canInteract)
    {
        isInteractable = canInteract; // This should be a field controlling interactability, ensure it exists and is used appropriately
    }


    //------------------
    //Open and Close HUD
    //------------------

    private void OpenInteractionPanel()
    {
        AudioManager.instance.Play("Positive Interact");

        if (PlayerInputHandler.Instance.GetCurrentControlScheme() == "Controller")
        {
            PlayerInputHandler.Instance.SwitchActionMap(true);
            addSeedButton.Select();
        }

        //Activate the panel and make it the currentVisible UI
        incubationPodHUDPanel.SetActive(true);
        UIController.Instance.m_CurrentUIVisible = incubationPodHUDPanel;


        DisplayIncubationHUDContents();

        //Set the text for the amount of days left
        daysRemainingText.text = daysLeft.ToString() + " days left \n to incubate";

    }
    private void CloseInteractionPrompt()
    {
        incubationPodHUDPanel.SetActive(false);

    }


    public void HandleAllChecks()
    {
        if(incubationState == IncubationState.OBJ_AddSeed)
        {
            Incubate();
        }
        else
        {
            ChangeState();
        }
    }

    public void Incubate()
    {
        if(CheckIfSeedInInventory())
        {
            ChangeState();
        }
        else
        {
            AudioManager.instance.Play("Negative Interact");
            Instantiate(needSeedPrefab, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity, incubationPodHUDPanel.transform);
            return;
        }
    }

    //this is a temporary fix for our issue
    private bool CheckIfSeedInInventory()
    {
        for (int i = 0; i < inventory.container.items.Length; i++)
        {
            if (inventory.container.items[i].id > -1 && inventory.container.items[i].amount > 0)
            {
                AudioManager.instance.Play("Insert 1");
                SetSeedData(inventory.container.items[i].id);
                inventory.AddItem(inventory.container.items[i].item, -1);
                if (inventory.container.items[i].amount <= 0)
                {
                    inventory.RemoveItem(inventory.container.items[i].item);
                }
                return true;
            }
        }
        return false;
    }

    private void SetSeedData(int index)
    {
        Debug.Log(index);
        AudioManager.instance.Play("Insert 1");
        seedIndex = index;
        dataDayCycle.incubationPodData[thisIndex].seedIndex = seedIndex;
    }

    
    //This function is called on the button in the inspector
    public void ChangeState()
    {
        incubationState = dataDayCycle.incubationPodData[thisIndex].incubationState;
        daysLeft = dataDayCycle.incubationPodData[thisIndex].daysLeft;
        //when the button is pressed, change the state to the next state and then display the new contents
        int i = (int)incubationState;
        i++;
        i %= 3;
        if (incubationState == IncubationState.OBJ_RemoveSeed)
        {
            SpawnAlien();
        }

     
        incubationState = (IncubationState)i;
      
        DisplayIncubationHUDContents();
        if (i == 0)
        {
            daysLeft = daysToIncubate;
            incubationState = IncubationState.OBJ_AddSeed;
        } 


        //Update the data
        dataDayCycle.incubationPodData[thisIndex].incubationState = incubationState;
        dataDayCycle.incubationPodData[thisIndex].daysLeft = daysLeft;
        
    }

    private void SpawnAlien()
    {
        if (T1alienPrefabs.Count > 0)
        {
            GameObject prefabToInstantiate = T1alienPrefabs[seedIndex]; // Get the prefab at the set index

            // Instantiate the prefab at a desired location and rotation
            Instantiate(prefabToInstantiate, spawnLocation, Quaternion.identity, AliensInWorld_Mono.instance.gameObject.transform);
            Debug.Log(prefabToInstantiate.name);
            seedIndex = -1;
        }
    }

    private void DisplayIncubationHUDContents()
    {
        //Hide Button
        switch(incubationState)
        {
            case IncubationState.OBJ_AddSeed:
                ShowAddSeedUI();
                break;

            case IncubationState.OBJ_Incubating:
                ShowIncubatingUI();
                break;

            case IncubationState.OBJ_RemoveSeed:
                ShowRemoveSeedUI();
                break;

            default:
                break;
        }
    }

    private void ShowAddSeedUI()
    {
        incubationState = IncubationState.OBJ_AddSeed;

        addSeedButton.gameObject.SetActive(true);
        buttonText.text = "Add Seed";

        seedImage.gameObject.SetActive(false);
        daysRemainingText.gameObject.SetActive(false);

        incubationLight.color = new Color(0, 1, 0);

        //Update the data
        incubationState = dataDayCycle.incubationPodData[thisIndex].incubationState;
        daysLeft = dataDayCycle.incubationPodData[thisIndex].daysLeft;
    }
    private void ShowIncubatingUI()
    {
        //Set the text for the amount of days left
        daysRemainingText.text = daysLeft.ToString() + " days left \n to incubate";
        incubationState = IncubationState.OBJ_Incubating;
        gameEvent.RaiseProgressChanged(ProgressState.SeedPlaced);
        addSeedButton.gameObject.SetActive(false);

        seedImage.gameObject.SetActive(true);
        daysRemainingText.gameObject.SetActive(true);
        incubationLight.color = new Color(1, 0, 0);

    }

    private void ShowRemoveSeedUI()
    {
        incubationState = IncubationState.OBJ_RemoveSeed;
        addSeedButton.gameObject.SetActive(true);
        buttonText.text = "Remove Seed";

        seedImage.gameObject.SetActive(false);
        daysRemainingText.gameObject.SetActive(false);

        incubationLight.color = new Color(0, 0, 1);
    }

    public override bool CheckIsInteractable() { return isInteractable; }
    public override bool IsTargetPointVisible() { return isInteractPointVisible; }
    public override bool FreezePlayerMovement() { return freezePlayerMovement; }
    public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }

}
