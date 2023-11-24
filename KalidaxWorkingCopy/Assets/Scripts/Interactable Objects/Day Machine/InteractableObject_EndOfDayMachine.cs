using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject_EndOfDayMachine : InteractableObject
{
    protected override void OnInteract()
    {
        PromptDayReset();
    }

    private void PromptDayReset()
    {
        //Are you sure you want to go to the next day?
        //Yes
        //No

        Debug.Log("Perform Day Cycle. 'NOT IMPLEMENTED YET'");
    }
}
