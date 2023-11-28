using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Clicked Interaction Event", menuName = "Event Senders/Clicked Interaction")]
public class SO_InteractableObject : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent clickedInteractButtonEvent = new UnityEvent();

    public void ClickedInteractButtonEventSend()
    {
        clickedInteractButtonEvent.Invoke();
    }

}
