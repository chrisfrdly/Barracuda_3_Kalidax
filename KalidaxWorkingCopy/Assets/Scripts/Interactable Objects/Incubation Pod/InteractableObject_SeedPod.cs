using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InteractableObject_SeedPod : InteractableObject
{
    private enum IncubationState
    {
        OBJ_AddSeed,
        OBJ_Incubating,
        OBJ_RemoveSeed
    }

    private IncubationState incubationState = IncubationState.OBJ_AddSeed;



    [Header("Panel")]
    [SerializeField] private GameObject incubationPodHUDPanel;

    [Header("Button")]
    [SerializeField] private Button addSeedButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Other Contents")]
    [SerializeField] private TextMeshProUGUI daysRemainingText;
    [SerializeField] private Image seedImage;
    [SerializeField] private SpriteRenderer incubationLight;

    [Header("Incubation Parameters")]
    [SerializeField] private int daysToIncubate = 7;
    private int daysLeft;

    //Properties
    public int m_DaysLeft { get => daysLeft; set => daysLeft = value; }

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
  
    protected override void OnInteract()
    {
        OpenInteractionPanel();

        HideUI();

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
        daysRemainingText.text = daysLeft.ToString();

        //If we started incubating, set the state to the incubating state
        if(daysLeft != daysToIncubate)
        {
            incubationState = IncubationState.OBJ_Incubating;
        }

    }
    private void CloseInteractionPrompt()
    {
        incubationPodHUDPanel.SetActive(false);

    }

    //This function is called on the button in the inspector
    public void ChangeState()
    {
        
        //when the button is pressed, change the state to the next state and then display the new contents
        int i = (int)incubationState;
        i++;
        i %= 3;

        incubationState = (IncubationState)i;
        
        DisplayIncubationHUDContents();
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
                throw new NotImplementedException();
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
    }
    private void ShowIncubatingUI()
    {


        incubationState = IncubationState.OBJ_Incubating;

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
