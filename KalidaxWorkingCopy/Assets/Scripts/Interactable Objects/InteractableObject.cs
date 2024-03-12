using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static SO_ControlSchemeHUD;

public abstract class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// Do not put this script in any object, this is purely to hold methods that other scripts will inherit and use
    /// Holds basic methods and variables that every interactable object will have
    /// </summary>

    //References
    [Header("Interactable Event Sender")]
    [SerializeField] protected SO_InteractableObject SO_interactableObject;
    [SerializeField] protected SO_ControlSchemeHUD SO_controlSchemeHUD;

    //Variables
    [Header("Variables")]
    [SerializeField] protected float tweenTime = 0.5f;
    [SerializeField] private GameObject interactPromptPanel; //The "Click E" button
    [SerializeField] private Image interactButtonSprite;
    protected GameObject interactedActor; //the object that interacted with this interact point

    [Separator()]
    [Title("Interaction Options", TextAlignment.Left, TextColour.White, 25)]
    //Variables for customization
    [Tooltip("Used to see if we can interact with this point at the current time. There might be a scenario where you can only interact with the point after completing a mission, if that's the case make this false from the start.")]
    [SerializeField] protected bool isInteractable = true;

    [Tooltip("If the player is in the target's range, should we reveal any UI or not? Even if the UI is hidden the object will still be interactable")]
    [SerializeField] protected bool isInteractPointVisible = true;

    [Tooltip("This determines whether the player needs to be looking at the interact point for it's HUD to display and for us to interact with it. True if you need to look at it, false if you don't need to be looking at it.")]
    [SerializeField] protected bool isRequiredToLookAtTarget = true;

    [Tooltip("Freezes the player's movement when they interact with this object")]
    [SerializeField] protected bool freezePlayerMovement = false;


    private PlayerInputHandler playerInputHandler;

    private bool inPlayerRange; //Keeps track if the player is in range or not



    //Properties
    public bool InPlayerRange { get => inPlayerRange; }

    /// <summary>
    /// In the "PlayerInteractWithObjects.cs" script, it checks if the player clicked the "interact" button on Update
    /// If they did click the "interact" Button, we send off an event through the "DS_SO_InteractableObject.cs" scriptable Object
    /// And ALL interactable objects receive the event, however the listener calls a method that checks to see if THIS OBJECT 
    /// is the one the player wants to interact with (by seeing if it's in range or not) and if so, then we run the OnInteract method.
    /// </summary>

    protected virtual void Awake()
    {

        SO_interactableObject.changedControlSchemeEvent.AddListener(OnControlsChanged);

        playerInputHandler = FindObjectOfType<PlayerInputHandler>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }


    /// <summary>
    /// Event is called from the "PlayerInputHandler" whenever a new control scheme is detected and is called here
    /// This goes in the scriptable object and finds the corresponding sprite and text to apply to the interact prompt
    /// </summary>
    private void OnControlsChanged(string _controlScheme)
    {
        if (interactButtonSprite == null) return;

        interactButtonSprite.sprite = SO_controlSchemeHUD.UpdateSpriteHUD(_controlScheme, SpriteType.Img_Interact);
    }

    /// <summary>
    /// Through the PlayerInteractScript.cs, the PlayerInRange() and PlayerOutOfRange() methods are called if the object is in range of the player
    /// </summary>

    #region Player In Range
    public virtual void OnPlayerEnterRange(float _a)
    {

        inPlayerRange = true;
        ShowUI();

    }

    private void ShowUI()
    {
        if (!IsTargetPointVisible()) return;

        //The UI is only shown if the child class declares the "IsTargetPointVisible" bool method true.
        interactPromptPanel.SetActive(true);

        //Tween animation

        LeanTween.scale(interactPromptPanel, Vector3.one, tweenTime);

    }


    #endregion

    #region Player Out of Range
    public virtual void OnPlayerExitRange()
    {
        inPlayerRange = false;

        if (IsTargetPointVisible())
        {
            //Play Animation
            LeanTween.scale(interactPromptPanel, Vector3.zero, tweenTime).setOnComplete(HideUI);
        }
    }

    private void HideUI()
    {
        interactPromptPanel.SetActive(false);
    }

    #endregion


    //abstract functions so ALL scripts that inherit from InteractableObject require this function

    public abstract void OnInteract(GameObject _interactedActor); //Determines what happens when the object is interacted with
    public abstract bool IsTargetPointVisible(); //If we want the UI to appear or not. Like animal Crossing if false, where they can still interact, but no UI
    public abstract bool IsInteractable(); //In case the user has missions to make them interactable
    public abstract bool FreezePlayerMovement(); //If we want to allow the player to move or not when in a conversation 
    public abstract bool IsRequiredToLookAtTarget(); //If we want the player to look at the interact point for it to display
                                                     //If true, we must look at it within a certain degrees
                                                     //If false, we can be looking in the opposite direction 
                                                     //This takes in 3D, so you might have to re-work the logic in DS_PlayerInteractWithObject for it to work in 2D
}
