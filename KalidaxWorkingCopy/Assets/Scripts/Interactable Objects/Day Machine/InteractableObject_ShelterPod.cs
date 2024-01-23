using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject_ShelterPod : InteractableObject
{

    protected override void OnInteract()
    {
        PromptDayReset();

        HideUI();
    }

    private void PromptDayReset()
    {
        //Are you sure you want to go to the next day?

        //Calls on the UI Controller to reveal the UI of the End of day Confirmation

        UIController.Instance.ShowEndOfDayConfirmationUI();
    }

    protected override bool IsInteractable() { return isInteractable; }
    protected override bool IsTargetPointVisible() { return isInteractPointVisible; }
    protected override bool FreezePlayerMovement() { return freezePlayerMovement; }
    public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }


}
