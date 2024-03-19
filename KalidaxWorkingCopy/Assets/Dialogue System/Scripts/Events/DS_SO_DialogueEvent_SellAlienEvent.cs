using DS_Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueContainer", menuName = "DialogueSystem/Events/Sell Alien Event")]
[System.Serializable]
public class DS_SO_DialogueEvent_SellAlienEvent : DS_SO_DialogueEvent
{
    public override void RunEvent()
    {
        //how do I pass in the alien in question?
        //I need a reference to the gameObject we're interacting with
        DS_GameEvents.Instance.CallSellAlienAction();
        base.RunEvent();

    }
}
