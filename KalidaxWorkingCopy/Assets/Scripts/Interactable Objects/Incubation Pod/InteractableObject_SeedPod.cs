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
    [Separator()]
    [SerializeField] private SO_Data_DayCycle dataDayCycle;

    [Header("Panel")]
    [SerializeField] private GameObject incubationPodHUDPanel;

    [Header("Button")]
    [SerializeField] private Button addSeedButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Other Contents")]
    [SerializeField] private TextMeshProUGUI daysRemainingText;
    [SerializeField] private Image seedImage;
    [SerializeField] private SpriteRenderer incubationLight;
    [SerializeField] private Vector3 spawnLocation;

    [Header("Incubation Parameters")]
    [SerializeField] private int daysToIncubate = 7;
    private int daysLeft;

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
            }



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
    protected override void OnInteract()
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
        if(PlayerInputHandler.Instance.GetCurrentControlScheme() == "Controller")
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
            InstantiateRandomPrefab();
        }

     
        incubationState = (IncubationState)i;
      
        DisplayIncubationHUDContents();
        if (i == 0) daysLeft = daysToIncubate;


        //Update the data
        Debug.Log("Updating Data");
        dataDayCycle.incubationPodData[thisIndex].incubationState = incubationState;
        dataDayCycle.incubationPodData[thisIndex].daysLeft = daysLeft;
        
    }

    private void InstantiateRandomPrefab()
    {
        if (T1alienPrefabs.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, T1alienPrefabs.Count); // Select a random index
            GameObject prefabToInstantiate = T1alienPrefabs[index]; // Get the prefab at the random index

            // Instantiate the prefab at a desired location and rotation
            Instantiate(prefabToInstantiate, spawnLocation, Quaternion.identity);
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

    protected override bool IsInteractable() { return isInteractable; }
    protected override bool IsTargetPointVisible() { return isInteractPointVisible; }
    protected override bool FreezePlayerMovement() { return freezePlayerMovement; }
    public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }

}
