using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject_ShelterPod : InteractableObject
{

    public override void OnInteract(GameObject _interactedActor)
    {

        PromptDayReset();

        AudioManager.instance.Play("Positive Interact");

    }

    private void PromptDayReset()
    {
        //Are you sure you want to go to the next day?

        //Calls on the UI Controller to reveal the UI of the End of day Confirmation

        UIController.Instance.ShowEndOfDayConfirmationUI();
    }

    public override bool CheckIsInteractable() { return isInteractable; }
    public override bool IsTargetPointVisible() { return isInteractPointVisible; }
    public override bool FreezePlayerMovement() { return freezePlayerMovement; }
    public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }


}
