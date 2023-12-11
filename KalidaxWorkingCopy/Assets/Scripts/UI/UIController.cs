using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Event Sender")]
    [SerializeField] private SO_InteractableObject SO_interactObject;
    public static UIController Instance;

    [Header("Parameters")]
    private GameObject currentUIVisible;

    [Header("End Of Day Confirmation UI")]
    [SerializeField] private GameObject endOfDayConfirmationUI;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public GameObject m_CurrentUIVisible { get => currentUIVisible; set => currentUIVisible = value; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        //Add events so when the player clicks a button, perform a function using lambda expressions
        yesButton.onClick.AddListener(() => ConfirmedDayReset());
        noButton.onClick.AddListener(() => CancelDayReset());
        SO_interactObject.clickedCancelButtonEvent.AddListener(CancelButtonPressed);

    }

    private void CancelButtonPressed()
    {
        Debug.Log("Pressed Cancel Button");
        SetActionMapInGame();
        endOfDayConfirmationUI.SetActive(false);
    }

    //This method is called from the "InteractableObject_EndOfDayMachine.cs" script
    //That function on the machine is called from the "PlayerInteractWithObject.cs" script when they press "E"
    public void ShowEndOfDayConfirmationUI()
    {
        endOfDayConfirmationUI.SetActive(true);
        currentUIVisible = endOfDayConfirmationUI;

        if (PlayerInputHandler.Instance.GetCurrentControlScheme() == "Controller")
            SetButton(yesButton);

    }

    private void ConfirmedDayReset()
    {
        //Now the Day Manager class will handle switching to the new day!
        DayManager.Instance.NewDay();
    }

    public void CancelDayReset()
    {
        SetActionMapInGame();
        endOfDayConfirmationUI.SetActive(false);
    }
    
    public void SetButton(Button _button)
    {
        _button.Select();
    }

    //will be callsed from the invubation pod UI
    public void SetActionMapInGame()
    {
        //if controller, then stop resume movement. If player, they can use the mouse
        if (PlayerInputHandler.Instance.GetCurrentControlScheme() == "Controller")
        {
            PlayerInputHandler.Instance.SwitchActionMap(false);
        }
    }

    public void HideCurrentUI()
    {
        if (currentUIVisible == null) return;
        currentUIVisible.SetActive(false);
        currentUIVisible = null;
    }

}
