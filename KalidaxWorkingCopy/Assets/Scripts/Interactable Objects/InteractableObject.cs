using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// Do not put this script in any object, this is purely to hold methods that other scripts will inherit and use
    /// Holds basic methods and variables that every interactable object will have
    /// </summary>

    //References
    [SerializeField] private SO_InteractableObject interactableObject;

    //Variables
    [Header("Variables")]
    [SerializeField] private GameObject interactUIPanel;
    [SerializeField] protected float tweenTime = 0.5f;

    /// <summary>
    /// In the "PlayerInteractWithObjects.cs" script, it checks if the player clicked the "E" button on Update
    /// If they did click the E Button, we send off an event through the "SO_InteractableObject.cs" scriptable Object
    ///And ALL interactable objects receive the event, however the listener calls a method that checks to see if THIS OBJECT 
    ///is the one the player wants to interact with (by seeing if it's UI is enabled) and if so, then we run the OnInteract method.
    /// </summary>
    
    private void Awake()
    {
        interactableObject.clickedInteractButtonEvent.AddListener(CheckIfUIActive);
    }

    private void CheckIfUIActive()
    {
        if(interactUIPanel.activeSelf == true)
        {
            OnInteract();
        }
    }

    protected abstract void OnInteract();

     /// <summary>
    /// Through the PlayerInteractScript.cs, the ShowUI() and HideUI() methods are called if the object is in range of the player
    /// </summary>
    public void ShowUI()
    {
        interactUIPanel.SetActive(true);

        //Tween animation
        interactUIPanel.transform.localScale = Vector3.one;
        LeanTween.scale(interactUIPanel, Vector3.one * 1.4f, tweenTime).setEasePunch();
    }

    public void HideUI()
    {
        interactUIPanel.SetActive(false);
        interactUIPanel.transform.localScale = Vector3.one;
    }

}
