using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InteractableObject_IncubationPod : InteractableObject
{

    [Header("Panel")]
    [SerializeField] private GameObject incubationPodHUDPanel;

    [Header("Button")]
    [SerializeField] private Button addSeedButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Other Contents")]
    [SerializeField] private TextMeshProUGUI daysRemainingText;
    [SerializeField] private Image seedImage;

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
    }
    private enum IncubationState
    {
        OBJ_AddSeed,
        OBJ_Incubating,
        OBJ_RemoveSeed
    }

    private IncubationState incubationState = IncubationState.OBJ_AddSeed;

    protected override void OnInteract()
    {
        OpenInteractionPrompt();
        HideInteractionPromptUI();
    }




    //------------------
    //Open and Close HUD
    //------------------
    private void OpenInteractionPrompt()
    {
        if(PlayerInputHandler.Instance.GetCurrentControlScheme() == "Controller")
        {
            PlayerInputHandler.Instance.SwitchActionMap(true);
            addSeedButton.Select();
        }

        incubationPodHUDPanel.SetActive(true);
        UIController.Instance.m_CurrentUIVisible = incubationPodHUDPanel;
        DisplayIncubationHUDContents();

    }
    private void CloseInteractionPrompt()
    {
        incubationPodHUDPanel.SetActive(false);

    }


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
    }
    private void ShowIncubatingUI()
    {
        incubationState = IncubationState.OBJ_Incubating;

        addSeedButton.gameObject.SetActive(false);

        seedImage.gameObject.SetActive(true);
        daysRemainingText.gameObject.SetActive(true);
    }

    private void ShowRemoveSeedUI()
    {
        incubationState = IncubationState.OBJ_RemoveSeed;

        addSeedButton.gameObject.SetActive(true);
        buttonText.text = "Remove Seed";

        seedImage.gameObject.SetActive(false);
        daysRemainingText.gameObject.SetActive(false);
    }

}
