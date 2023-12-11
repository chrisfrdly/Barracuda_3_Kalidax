using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject_ShelterPod : InteractableObject
{
    protected override void OnInteract()
    {
        PromptDayReset();
        HideInteractionPromptUI();
    }

    private void PromptDayReset()
    {
        //Are you sure you want to go to the next day?

        //Calls on the UI Controller to reveal the UI of the End of day Confirmation

        UIController.Instance.ShowEndOfDayConfirmationUI();
    }

}
