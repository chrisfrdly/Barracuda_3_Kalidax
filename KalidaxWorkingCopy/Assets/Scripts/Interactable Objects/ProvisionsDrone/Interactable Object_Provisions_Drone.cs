using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject_Provisions_Drone : InteractableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //these are the necessary UI things for this object I don't actually know what they do yet
    protected override void OnInteract()
    {

    }

    protected override bool IsInteractable() { return isInteractable; }
    protected override bool IsTargetPointVisible() { return isInteractPointVisible; }
    protected override bool FreezePlayerMovement() { return freezePlayerMovement; }
    public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }
}
