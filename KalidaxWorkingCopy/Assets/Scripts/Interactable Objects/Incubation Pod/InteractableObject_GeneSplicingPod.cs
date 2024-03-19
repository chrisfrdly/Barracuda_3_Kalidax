using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class InteractableObject_GeneSplicingPod : InteractableObject
{
    [Separator()]
    [SerializeField] private SO_AliensInWorld aliens;
    [SerializeField] private UIAlienGridList UIalienGridList;

    [Header("Incubation Pod HUD Panel")]
    [SerializeField] private GameObject incubationPodHUDPanel;
    [SerializeField] private GameObject alienGridPanel;

    [Header("Incubation Pod Buttons")]
    [SerializeField] private Button[] addAlienButton = new Button[2];
    private int buttonModified;

    //aliens that are assigned to the buttons
    [SerializeField] private SO_Alien[] aliensAdded = new SO_Alien[2];

    //Properties
    public SO_Alien[] m_AliensAdded { get => aliensAdded;}


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

        addAlienButton[0].onClick.AddListener(() => ButtonClicked(0));
        addAlienButton[1].onClick.AddListener(() => ButtonClicked(1));

        //Need to clear the List every awake or else will stack with every play.
        //Cannot clear in the "UIAlienGrisList" since it's awake is called after
        //the aliens are added to the list, so they will be removed
        aliens.worldAliens.Clear();

    }

    public override void OnInteract(GameObject _interactedActor)
    {
        OpenInteractionPanel();

        AudioManager.instance.Play("Positive Interact");
    }


    //------------------
    //Open and Close HUD
    //------------------

    private void OpenInteractionPanel()
    {
        if (PlayerInputHandler.Instance.GetCurrentControlScheme() == "Controller")
        {
            PlayerInputHandler.Instance.SwitchActionMap(true);
            addAlienButton[0].Select();
        }

        //Activate the panel and make it the currentVisible UI
        incubationPodHUDPanel.SetActive(true);
        UIController.Instance.m_CurrentUIVisible = incubationPodHUDPanel;

        //set an event for the buttons. If a button is clicked, reveal the grid panel


    }
    private void CloseInteractionPrompt()
    {
        incubationPodHUDPanel.SetActive(false);

    }

    //passing in an int value so we can do more than just modify the button
    private void ButtonClicked(int _button)
    {
        AudioManager.instance.Play("Insert 1");

        alienGridPanel.SetActive(true);
        buttonModified = _button;
    }
    
    //Called from the UIAlienGridList.cs
    public void SetAlien(SO_Alien a)
    {
        //Call item select sound class
        AudioManager.instance.Play("Insert 2");

        //Change the button to the sprite of the alien
        addAlienButton[buttonModified].GetComponent<Image>().sprite = a.m_AlienSprite;
        aliensAdded[buttonModified] = a;

        //For controllers, we have to re-select the + button that they previously clicked
        addAlienButton[buttonModified].Select();

        //Remove the "+" Text on the button if an alien is already slotted 
        addAlienButton[buttonModified].GetComponentInChildren<TextMeshProUGUI>().text = "";


        if (aliensAdded[0] != null && aliensAdded[1] != null)
        {
            CheckPossibleCombos();
        }
    }

    private void CheckPossibleCombos()
    {
        //Check through all tier 2, 3, and 4
        
    }

    public override bool CheckIsInteractable() { return isInteractable; }
    public override bool IsTargetPointVisible() { return isInteractPointVisible; }
    public override bool FreezePlayerMovement() { return freezePlayerMovement; }
    public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }

}
